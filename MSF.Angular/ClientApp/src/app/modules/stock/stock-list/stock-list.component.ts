import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { Stock } from '../models/stock';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { StockService } from '../services/stock.service';
import { StockFormComponent } from '../stock-form/stock-form.component';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { fromEvent, of as observableOf } from 'rxjs';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-stock-list',
  templateUrl: './stock-list.component.html',
  styleUrls: ['./stock-list.component.css']
})
export class StockListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['date', 'productName', 'unitPrice', 'amount', 'providerName', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  stocks: Stock[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _stockService: StockService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  openDialog(stock: Stock = null) {
    const dialogRef = this.dialog.open(StockFormComponent, {
      disableClose: true,
      data: stock
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Estoque");
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
    this._stockService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.stocks = data.stocks;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(stock: Stock) {
    const message = `Deseja Estornar o Estoque: ${stock.providerName} - ${stock.productName}?`;

    const data = new ConfirmDialogModel("ESTORNAR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._stockService.delete(stock.id)
          .subscribe(() => {
            this.openSnackBarBottom('Estoque estornado com sucesso!', 'ESTOQUE');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao estornar estoque: ${error.message}`, 'ESTOQUE');
          });
      }
    });
  }
}
