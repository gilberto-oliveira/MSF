import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Shop } from '../../shop/models/shop';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ShopService } from './../../shop/services/shop.service';
import { ShopFormComponent } from './../../shop/shop-form/shop-form.component';
import { WorkCenterService } from '../services/work-center.service';
import { WorkCenter } from '../models/work-center';
import { filter, tap, debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-work-center-form',
  templateUrl: './work-center-form.component.html',
  styleUrls: ['./work-center-form.component.css']
})
export class WorkCenterFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  workCenterForm: FormGroup;
  shopSelectSearch = new FormControl();
  shopSearching = false;
  public shops: Observable<Shop[]> = new Observable<Shop[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _workCenterService: WorkCenterService,
    public dialogRef: MatDialogRef<ShopFormComponent>,
    @Inject(MAT_DIALOG_DATA) public workCenter: WorkCenter,
    private fb: FormBuilder,
    private _shopService: ShopService) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.workCenterForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(10)]],
      description: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]],
      shopId: ['', [Validators.required]]
    });
    this.onStateFilter();
  }

  ngAfterViewInit() {
    if (this.workCenter != null) {
      setTimeout(() => {
        this.workCenterForm.patchValue(this.workCenter);
        this.shopSelectSearch.patchValue(this.workCenter.shopName);
      }, 10);
    }
  }

  onSave() {
    const workCenter = this.workCenterForm.value;
    if (this.workCenter != null) {
      workCenter.id = this.workCenter.id;
      this._workCenterService.edit(workCenter)
        .subscribe(() => {
          this.openSnackBarBottom('Caixa editado com sucesso!', 'CAIXAS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar caixa: ${error.message}`, 'CAIXAS');
        });
    } else {
    this._workCenterService.create(workCenter)
      .subscribe(() => {
        this.openSnackBarBottom('Caixa criado com sucesso!', 'CAIXAS');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`Erro ao criar caixa: ${error.message}`, 'CAIXAS');
      });
    }
  }

  onStateFilter() {
    this.shopSelectSearch.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.shopSearching = true),
        debounceTime(1000),
        map(search => {
          return this._shopService.find(search);
        }))
      .subscribe(filteredBanks => {
        this.shopSearching = false;
        this.shops = filteredBanks;
      },
        error => {
          this.shopSearching = false;
        });
  }

}
