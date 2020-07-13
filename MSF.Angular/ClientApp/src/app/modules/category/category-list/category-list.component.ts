import { Component, OnInit, ViewChild, AfterViewInit, ElementRef } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar, MatPaginator, MatDialog } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { catchError, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { of as observableOf, fromEvent } from 'rxjs';
import { CategoryService } from '../services/category.service';
import { CategoryFormComponent } from './../category-form/category-form.component';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { Category } from './../models/category';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['code', 'description', 'subcategories', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  categories: Category[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private categoryService: CategoryService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  openDialog(category: Category = null) {
    const dialogRef = this.dialog.open(CategoryFormComponent, {
      disableClose: true,
      data: category
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Categorias");
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
    this.categoryService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.categories = data.categories;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(category: Category) {
    const message = `Deseja Excluir a Categoria: ${category.description}?`;

    const data = new ConfirmDialogModel("EXCLUIR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.categoryService.delete(category.id)
          .subscribe(() => {
            this.openSnackBarBottom('Categoria excluída com sucesso!', 'CATEGORIAS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir categoria: ${error.message}`, 'CATEGORIAS');
          });
      }
    });
  }

}
