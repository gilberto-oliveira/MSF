import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { WorkCenter } from '../models/work-center';
import { of as observableOf, fromEvent  } from 'rxjs';
import { catchError, tap, debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { WorkCenterService } from './../services/work-center.service';
import { WorkCenterFormComponent } from './../work-center-form/work-center-form.component';

@Component({
  selector: 'app-work-center-list',
  templateUrl: './work-center-list.component.html',
  styleUrls: ['./work-center-list.component.css']
})
export class WorkCenterListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['code', 'description', 'shopName', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  workcenters: WorkCenter[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
              protected _titleService: NavigationTitleService,
              private workCenterService: WorkCenterService,
              public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Caixas");
    this.getLazy('', this.pageSize);
  }

  openDialog(workCenter: WorkCenter = null) {
    const dialogRef = this.dialog.open(WorkCenterFormComponent, {
      disableClose: true,
      data: workCenter
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
    this.workCenterService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.workcenters = data.workCenters;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(workCenter: WorkCenter) {
    const message = `Deseja Excluir o Caixa: ${workCenter.description}?`;

    const data = new ConfirmDialogModel("EXCLUIR", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.workCenterService.delete(workCenter.id)
          .subscribe(() => {
            this.openSnackBarBottom('Caixa excluído com sucesso!', 'CAIXAS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir Caixa: ${error.message}`, 'CAIXAS');
          });
      }
    });
  }
}
