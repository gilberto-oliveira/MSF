import { Component, OnInit, AfterViewInit, ViewChild, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Observable } from 'rxjs';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product';
import { CategoryService } from './../../category/services/category.service';
import { filter, tap, debounceTime, map } from 'rxjs/operators';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  productForm: FormGroup;
  subcategorySelectSearch = new FormControl();
  subcategorySearching = false;
  public subcategories: Observable<any[]> = new Observable<any[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _productService: ProductService,
    public dialogRef: MatDialogRef<ProductFormComponent>,
    @Inject(MAT_DIALOG_DATA) public product: Product,
    private fb: FormBuilder,
    private _categoryService: CategoryService) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.productForm = this.fb.group({
      description: ['', [Validators.required, Validators.maxLength(100)]],
      profit: [0, [Validators.required]],
      subcategoryId: ['', [Validators.required]]
    });
    this.onStateFilter();
  }

  ngAfterViewInit() {
    if (this.product != null) {
      setTimeout(() => {
        this.productForm.patchValue(this.product);
        this.subcategorySelectSearch.patchValue(this.product.subcategoryName);
      }, 10);
    }
  }

  onSave() {
    const product = this.productForm.value;
    if (this.product != null) {
      product.id = this.product.id;
      this._productService.edit(product)
        .subscribe(() => {
          this.openSnackBarBottom('Produto editado com sucesso!', 'PRODUTOS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar produto: ${error.message}`, 'PRODUTOS');
        });
    } else {
    this._productService.create(product)
      .subscribe(() => {
        this.openSnackBarBottom('Produto criado com sucesso!', 'PRODUTOS');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`Erro ao criar produto: ${error.message}`, 'PRODUTOS');
      });
    }
  }

  onStateFilter() {
    this.subcategorySelectSearch.valueChanges
      .pipe(
        filter(search => !!search),
        tap(() => this.subcategorySearching = true),
        debounceTime(1000),
        map(search => {
          return this._categoryService.find(search);
        }))
      .subscribe(filteredBanks => {
        this.subcategorySearching = false;
        this.subcategories = filteredBanks;
      },
        error => {
          this.subcategorySearching = false;
        });
  }

}
