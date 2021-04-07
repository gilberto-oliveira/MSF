import { AfterViewInit, Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MatSnackBar, MAT_DIALOG_DATA } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { Operation } from '../models/operation';
import { OperationService } from './../services/operation.service';
import { Observable } from 'rxjs';
import { debounceTime, filter, map, tap } from 'rxjs/operators';
import { StockService } from './../../stock/services/stock.service';
import { Product } from '../../product/models/product';
import { Provider } from './../../provider/models/provider';

@Component({
  selector: 'app-sale-process-form',
  templateUrl: './sale-process-form.component.html',
  styleUrls: ['./sale-process-form.component.css']
})
export class SaleProcessFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  operationForm: FormGroup;
  providerSelectSearch = new FormControl();
  providerSearching = false;
  public providers: Observable<Provider[]> = new Observable<Provider[]>();

  productSelectSearch = new FormControl();
  productSearching = false;
  public products: Observable<Product[]> = new Observable<Product[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _operationService: OperationService,
    public dialogRef: MatDialogRef<SaleProcessFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private _stockService: StockService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this.operationForm = this.fb.group({
      date: ['', [Validators.required]],
      productId: ['', [Validators.required]],
      providerId: ['', [Validators.required]],
      unitPrice: [0, [Validators.required]],
      amount: [null, [Validators.required]]
    });
    this.onProductFilter();
    this.onProviderFilter();
  }

  ngAfterViewInit() {
    const operationToEdit = this.data.operation;
    if (operationToEdit != null) {
      setTimeout(() => {
        this.operationForm.patchValue(operationToEdit);
        this.productSelectSearch.patchValue(operationToEdit.productName);
        this.providerSelectSearch.patchValue(operationToEdit.providerName);
      }, 10);
    }
  }

  onSave() {
    const operationToEdit = this.data.operation;
    const operation = this.operationForm.value as Operation;
    operation.workCenterControlId = this.data.workCenterControlId;
    operation.type = 'A';
    if (operationToEdit != null) {
      operation.id = operationToEdit.id;
      this._operationService.edit(operation)
        .subscribe(() => {
          this.openSnackBarBottom('Operação editada com sucesso!', 'OPERAÇÕES');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`${error.detail}`, 'OPERAÇÕES');
        });
    } else {
    this._operationService.create(operation)
      .subscribe(() => {
        this.openSnackBarBottom('Operação criada com sucesso!', 'OPERAÇÕES');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'OPERAÇÕES');
      });
    }
  }

  onProductFilter() {
    this.productSelectSearch.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.productSearching = true),
        debounceTime(1000),
        map(search => {
          return this._stockService.findProductByFilter(search);
        }))
      .subscribe(filteredBanks => {
        this.productSearching = false;
        this.products = filteredBanks;
      },
        error => {
          this.productSearching = false;
        });
  }

  onProviderFilter() {
    this.providerSelectSearch.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.providerSearching = true),
        debounceTime(1000),
        map(search => {
          const operation = this.operationForm.value as Operation;
          if (operation.productId > 0) {
            return this._stockService.findProviderByFilterAndProduct(search, operation.productId);
          } else {
            this.openSnackBarTop('Selecione um produto', 'OPERAÇÕES');
            return new Observable<Provider[]>();
          }
        }))
      .subscribe(filteredBanks => {
        this.providerSearching = false;
        this.providers = filteredBanks;
      },
        error => {
          this.providerSearching = false;
        });
  }

  setUnitPrice() {
    const operation = this.operationForm.value as Operation;
    this._stockService.findTotalPriceByProductAndProvider(operation.productId, operation.providerId)
      .subscribe(price => {
        this.operationForm.get('unitPrice').patchValue(price);
      },
        error => {
          this.openSnackBarTop(`Erro ao buscar preço do produto - ${error.detail}`, 'OPERAÇÕES');
      });    
  }

}