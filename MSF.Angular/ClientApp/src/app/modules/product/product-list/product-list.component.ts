import { Component, OnInit, AfterViewInit, ViewChild, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { Product } from './../models/product';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ProductService } from './../services/product.service';
import { ProductFormComponent } from '../product-form/product-form.component';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { fromEvent, of as observableOf } from 'rxjs';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['description', 'profit', 'subcategory', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  products: Product[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _productService: ProductService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  openDialog(product: Product = null) {
    const dialogRef = this.dialog.open(ProductFormComponent, {
      disableClose: true,
      data: product
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Produtos");
    this.getLazy('', this.pageSize);
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
    this._productService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.products = data.products;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(product: Product) {
    const message = `Deseja Excluir o Produto: ${product.description}?`;

    const data = new ConfirmDialogModel("EXCLUIR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._productService.delete(product.id)
          .subscribe(() => {
            this.openSnackBarBottom('Produto excluído com sucesso!', 'PRODUTOS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir produto: ${error.message}`, 'PRODUTOS');
          });
      }
    });
  }
}
