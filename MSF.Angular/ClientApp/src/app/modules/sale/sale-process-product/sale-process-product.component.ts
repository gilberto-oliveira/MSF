import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { Operation } from '../models/operation';
import { catchError, tap } from 'rxjs/operators';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { SaleProcessFormComponent } from '../sale-process-form/sale-process-form.component';
import { of as observableOf } from 'rxjs';
import { OperationService } from '../services/operation.service';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { WorkCenterControl } from '../models/work-center-control';
import { MatDialog, MatPaginator, MatSnackBar } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-sale-process-product',
  templateUrl: './sale-process-product.component.html',
  styleUrls: ['./sale-process-product.component.css']
})
export class SaleProcessProductComponent extends BaseComponent implements OnInit {

  @Input() workCenterControl: WorkCenterControl;

  displayedColumns: string[] = ['productName', 'providerName', 'amount', 'unitPrice', 'total', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  operations: Operation[] = [];
  resultsLength: number = 0;
  pageSize = 10;
  totalPrice: number = 0;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _operationService: OperationService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this.getLazy('A', '', this.pageSize);
    this.setPaginatorEvent();
  }

  setPaginatorEvent() {
    this.paginator.page
      .pipe(
        tap(() => this.getLazy('A', '', this.paginator.pageSize, this.paginator.pageIndex * this.paginator.pageSize))
      ).subscribe();
  }

  getLazy(type: string, filter: string, take: number, skip: number = 0) {
    const id = this.workCenterControl.id;
    
    this._operationService.lazy(id, type, filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.operations = data.operations;
      }),
      catchError(() => {
        return observableOf([]);
      });

    this._operationService.findTotalPriceByWorkCenterControlAndType(id, type)
      .subscribe(price => {
        this.totalPrice = price;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  openDialog(operation: Operation = null) {
    const dialogRef = this.dialog.open(SaleProcessFormComponent, {
      disableClose: true,
      data: { operation: operation, workCenterControlId: this.workCenterControl.id }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('A', '', this.pageSize);
      }
    });
  }

  confirmDialog(operation: Operation) {
    const message = `Deseja remover a Operação: ${operation.providerName} - ${operation.productName}?`;

    const data = new ConfirmDialogModel("REMOVER", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._operationService.delete(operation.id)
          .subscribe(() => {
            this.openSnackBarBottom('Operação removida com sucesso!', 'OPERAÇÕES');
            this.getLazy('A', '', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao remover operação: ${error.message}`, 'OPERAÇÕES');
          });
      }
    });
  }
}
