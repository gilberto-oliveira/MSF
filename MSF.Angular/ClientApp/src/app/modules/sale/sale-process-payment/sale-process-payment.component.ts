import { Component, Input, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { WorkCenterControl } from './../models/work-center-control';
import { MatDialog, MatSnackBar, MatStepper } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OperationService } from '../services/operation.service';
import { catchError } from 'rxjs/operators';
import { of as observableOf } from 'rxjs';
import { WorkCenterControlService } from '../services/work-center-control.service';

@Component({
  selector: 'app-sale-process-payment',
  templateUrl: './sale-process-payment.component.html',
  styleUrls: ['./sale-process-payment.component.css']
})
export class SaleProcessPaymentComponent extends BaseComponent implements OnInit {

  @Input() workCenterControl: WorkCenterControl;
  @Input() stepper: MatStepper;

  paymentForm: FormGroup;
  totalPrice: number = 0;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private fb: FormBuilder,
    private _operationService: OperationService,
    private _workCenterControlService: WorkCenterControlService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this.paymentForm = this.fb.group({
      paymentMethod: ['', [ Validators.required ]],
    });
    this.stepper.selectionChange.subscribe(() => this.getTotalPrice());
  }

  finishSaleProcess() {
    const data = new ConfirmDialogModel("FINALIZAR VENDA", "Não esqueça de receber o pagamento antes de finalizar. Deseja finalizar a venda atual?");
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const id = this.workCenterControl.id;
        this._workCenterControlService.finishSaleProcess(id)
          .subscribe(() => {
            this.openSnackBarBottom('Venda finalizada com sucesso!', 'VENDAS');
            this.stepper.reset();
          }, error => {
            this.openSnackBarTop(`Erro ao finalizar venda: ${error.message}`, 'VENDAS');
          });
      }
    });
  }

  getTotalPrice() {
    const id = this.workCenterControl.id;
    const type = 'A';
    this._operationService.findTotalPriceByWorkCenterControlAndType(id, type)
      .subscribe(price => {
        this.totalPrice = price;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }
}
