import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA, MAT_DATE_LOCALE } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ProductService } from '../../product/services/product.service';
import { ProviderService } from '../../provider/services/provider.service';
import { Stock } from '../models/stock';
import { StockService } from '../services/stock.service';
import { filter, tap, debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-stock-form',
  templateUrl: './stock-form.component.html',
  styleUrls: ['./stock-form.component.css']
})
export class StockFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  stockForm: FormGroup;
  
  providerSelectSearch = new FormControl();
  providerSearching = false;
  public providers: Observable<any[]> = new Observable<any[]>();

  productSelectSearch = new FormControl();
  productSearching = false;
  public products: Observable<any[]> = new Observable<any[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _productService: ProductService,
    private _providerService: ProviderService,
    public dialogRef: MatDialogRef<StockFormComponent>,
    @Inject(MAT_DIALOG_DATA) public stock: Stock,
    private fb: FormBuilder,
    private _stockService: StockService) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.stockForm = this.fb.group({
      date: ['', [Validators.required]],
      productId: ['', [Validators.required]],
      providerId: ['', [Validators.required]],
      unitPrice: [0, [Validators.required]],
      amount: [0, [Validators.required]]
    });
    this.onProductFilter();
    this.onProviderFilter();
  }

  ngAfterViewInit() {
    if (this.stock != null) {
      setTimeout(() => {
        this.stockForm.patchValue(this.stock);
        this.productSelectSearch.patchValue(this.stock.productName);
        this.providerSelectSearch.patchValue(this.stock.providerName);
      }, 10);
    }
  }

  onSave() {
    const stock = this.stockForm.value;
    if (this.stock != null) {
      stock.id = this.stock.id;
      this._stockService.edit(stock)
        .subscribe(() => {
          this.openSnackBarBottom('Estoque editado com sucesso!', 'ESTOQUES');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar estoque: ${error.message}`, 'ESTOQUES');
        });
    } else {
    this._stockService.create(stock)
      .subscribe(() => {
        this.openSnackBarBottom('Estoque criado com sucesso!', 'ESTOQUES');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`Erro ao criar estoque: ${error.message}`, 'PRODUTOS');
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
          return this._productService.find(search);
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
          return this._providerService.find(search);
        }))
      .subscribe(filteredBanks => {
        this.providerSearching = false;
        this.providers = filteredBanks;
      },
        error => {
          this.providerSearching = false;
        });
  }

}
