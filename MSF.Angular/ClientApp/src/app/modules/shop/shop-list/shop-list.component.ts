import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar, MatPaginator, MatDialog } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { Shop } from './../models/shop';
import { ShopService } from '../services/shop.service';
import { ShopFormComponent } from '../shop-form/shop-form.component';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { fromEvent, of as observableOf } from 'rxjs';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-shop-list',
  templateUrl: './shop-list.component.html',
  styleUrls: ['./shop-list.component.css']
})
export class ShopListComponent extends BaseComponent implements OnInit {

  displayedColumns: string[] = ['code', 'description', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  shops: Shop[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private shopService: ShopService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  openDialog(shop: Shop = null) {
    const dialogRef = this.dialog.open(ShopFormComponent, {
      disableClose: true,
      data: shop
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Lojas");
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
    this.shopService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.shops = data.shops;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(shop: Shop) {
    const message = `Deseja Excluir a Loja: ${shop.description}?`;

    const data = new ConfirmDialogModel("EXCLUIR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });
    
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.shopService.delete(shop.id)
          .subscribe(() => {
            this.openSnackBarBottom('Loja excluída com sucesso!', 'LOJAS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir loja: ${error.message}`, 'LOJAS');
          });
      }
    });
  }

}
