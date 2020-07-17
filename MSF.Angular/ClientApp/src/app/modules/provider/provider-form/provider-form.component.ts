import { Component, OnInit, Inject, ViewChild, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA, MatSlideToggleChange, MatSlideToggle } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { Provider } from './../models/provider';
import { ProviderService } from './../services/provider.service';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { State } from 'src/app/shared/models/state';
import { filter, tap, debounceTime, map, delay } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { StateService } from './../../../shared/services/state.service';

@Component({
  selector: 'app-provider-form',
  templateUrl: './provider-form.component.html',
  styleUrls: ['./provider-form.component.css']
})
export class ProviderFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  providerForm: FormGroup;
  public mask: string = '000.000.000-00';
  @ViewChild(MatSlideToggle, { static: true }) slideToggle: MatSlideToggle;
  stateSelectSearch = new FormControl();
  stateSearching = false;
  public states: Observable<State[]> = new Observable<State[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _providerService: ProviderService,
    public dialogRef: MatDialogRef<ProviderFormComponent>,
    @Inject(MAT_DIALOG_DATA) public provider: Provider,
    private fb: FormBuilder,
    private _stateService: StateService) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.providerForm = this.fb.group({
      name: ['', [Validators.required]],
      code: ['', [Validators.required]],
      stateId: ['', [Validators.required]]
    });
    this.onStateFilter();
  }

  ngAfterViewInit() {
    if (this.provider != null) {
      setTimeout(() => {
        this.slideToggle.checked = this.provider.code.length > 11;
        this.mask = this.slideToggle.checked ? '00.000.000/0000-00' : '000.000.000-00';
        this.providerForm.patchValue(this.provider);
        this.stateSelectSearch.patchValue(this.provider.stateName);
      }, 10);
    }
  }

  changeMask(event: MatSlideToggleChange) {
    const checked = event.checked;
    this.mask = checked ? '00.000.000/0000-00' : '000.000.000-00';
    this.providerForm.get('code').patchValue('');
  }

  onSave() {
    const provider = this.providerForm.value;
    if (this.provider != null) {
      provider.id = this.provider.id;
      this._providerService.edit(provider)
        .subscribe(() => {
          this.openSnackBarBottom('Fornecedor editado com sucesso!', 'FORNECEDORES');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar fornecedor: ${error.message}`, 'FORNECEDORES');
        });
    } else {
    this._providerService.create(provider)
      .subscribe(() => {
        this.openSnackBarBottom('Fornecedor criado com sucesso!', 'FORNECEDORES');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`Erro ao criar fornecedor: ${error.message}`, 'FORNECEDORES');
      });
    }
  }

  onStateFilter() {
    this.stateSelectSearch.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.stateSearching = true),
        debounceTime(1000),
        map(search => {
          return this._stateService.lazyStates(search);
        }))
      .subscribe(filteredBanks => {
        this.stateSearching = false;
        this.states = filteredBanks;
      },
        error => {
          this.stateSearching = false;
        });
  }
}
