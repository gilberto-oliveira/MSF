import { Component, OnInit, ElementRef, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator, MatSnackBar, MatDialog } from '@angular/material';
import { Role } from './../../auth/models/role';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { RoleService } from '../services/role.service';
import { tap, debounceTime, distinctUntilChanged, catchError } from 'rxjs/operators';
import { fromEvent, of as observableOf } from 'rxjs';
import { ConfirmDialogModel } from 'src/app/shared/models/confirm-dialog-model';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ActivatedRoute } from '@angular/router';
import { RoleFormComponent } from '../role-form/role-form.component';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-role-list',
  templateUrl: './role-list.component.html',
  styleUrls: ['./role-list.component.css']
})
export class RoleListComponent extends BaseComponent implements AfterViewInit, OnInit {

  displayedColumns: string[] = ['name', 'options'];
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild('input', { static: true }) input: ElementRef;
  roles: Role[];
  resultsLength: number = 0;
  pageSize = 10;
  userId: number = null;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _roleService: RoleService,
    private _userService: UserService,
    private route: ActivatedRoute,
    public dialog: MatDialog) {
    super(snackBar, _titleService);
    this.setupUser();
  }

  setupUser() {
    this.userId = this.route.snapshot.params.id;
  }

  openDialog() {
    const dialogRef = this.dialog.open(RoleFormComponent, {
      disableClose: true,
      data: this.userId
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getLazy('', this.pageSize);
      }
    });
  }

  ngOnInit() {
    this.getLazy('', this.pageSize);
    this.getCurrentUser();
  }

  getCurrentUser() {
    this._userService.lazyById(this.userId)
      .subscribe(u => { 
        this._titleService.setTitle("Perfis do Usuário: " + u.firstName + " " + u.lastName);
       }),
      catchError(() => {
        return observableOf([]);
      })
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
    this._roleService.lazyByUser(filter, take, skip, this.userId)
      .subscribe(data => {
        this.resultsLength = data.count;
        this.roles = data.roles;
      }),
      catchError(() => {
        return observableOf([]);
      });
  }

  confirmDialog(role: Role) {
    const message = `Deseja excluir perfil: ${role.name}?`;

    const data = new ConfirmDialogModel("EXCLUIR PERFIL", message);

    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._roleService.delete(this.userId, role.id)
          .subscribe(() => {
            this.openSnackBarBottom('Perfil excluído com sucesso!', 'PERFIS');
            this.getLazy('', this.pageSize);
          }, error => {
            this.openSnackBarTop(`Erro ao excluir perfil: ${error.message}`, 'PERFIS');
          });
      }
    });
  }

}
