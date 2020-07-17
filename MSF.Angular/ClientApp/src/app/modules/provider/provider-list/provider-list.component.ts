import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar, MatPaginator, MatDialog } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { Provider } from '../models/provider';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { of as observableOf, fromEvent } from 'rxjs';
import { ProviderService } from './../services/provider.service';
import { MaskPipe } from 'ngx-mask';
import { ProviderFormComponent } from '../provider-form/provider-form.component';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-provider-list',
  templateUrl: './provider-list.component.html',
  styleUrls: ['./provider-list.component.css'],
  providers: [MaskPipe]
})
export class ProviderListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['name', 'code', 'stateName', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  providers: Provider[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
              protected _titleService: NavigationTitleService,
              private maskPipe: MaskPipe,
              private providerService: ProviderService,
              public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Fornecedores");
    this.getLazy('', this.pageSize);
  }

  public formatCode(code: string): string {
    const provider = code.length > 11 ? '00.000.000/0000-00' : '000.000.000-00';
    return this.maskPipe.transform(code, provider);
  }

  openDialog(provider: Provider = null) {
    const dialogRef = this.dialog.open(ProviderFormComponent, {
      disableClose: true,
      data: provider
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngAfterViewInit() {
    this.paginator.page
      .pipe(
        tap(() => this.getLazy('', this.paginator.pageSize, this.paginator.pageIndex * this.paginator.pageSize))
      ).subscribe();

    fromEvent(this.input.nativeElement, 'keyup')
      .pipe(
        debounceTime(150),
        distinctUntilChanged(),
        tap(() => {
          const filter = this.input.nativeElement.value.trim().toLowerCase();
          this.getLazy(filter, this.paginator.pageSize)
        })
      ).subscribe();
  }

  getLazy(filter: string, take: number, skip: number = 0) {
    this.providerService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.providers = data.providers;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(provider: Provider) {
    const message = `Deseja Excluir o Fornecedor: ${provider.name}?`;

    const data = new ConfirmDialogModel("EXCLUIR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.providerService.delete(provider.id)
          .subscribe(() => {
            this.openSnackBarBottom('Fornecedor excluído com sucesso!', 'FORNECEDORES');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir Fornecedor: ${error.message}`, 'FORNECEDORES');
          });
      }
    });
  }

}
