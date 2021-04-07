import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { User } from './../../auth/models/user';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { UserService } from '../services/user.service';
import { UserFormComponent } from './../user-form/user-form.component';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { fromEvent, of as observableOf } from 'rxjs';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['userName', 'firstName', 'lastName', 'email', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  users: User[];
  resultsLength: number = 0;
  pageSize = 10;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _userService: UserService,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
  }

  openDialog(user: User = null) {
    const dialogRef = this.dialog.open(UserFormComponent, {
      disableClose: true,
      data: user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Usuários");
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
    this._userService.getLazy(filter, take, skip)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.users = data.users;
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'USUÁRIOS');
      });
  }

  confirmDialog(user: User) {
    const message = `Deseja resetar a senha do usuário: ${user.firstName} ${user.lastName}?`;

    const data = new ConfirmDialogModel("RESETAR SENHA", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._userService.resetPassword(user.id)
          .subscribe(() => {
            this.openSnackBarBottom('Senha resetada com sucesso!', 'USUÁRIOS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`${error.detail}`, 'USUÁRIOS');
          });
      }
    });
  }

}
