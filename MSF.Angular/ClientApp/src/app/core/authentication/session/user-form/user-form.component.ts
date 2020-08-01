import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { UserService } from '../services/user.service';
import { User } from './../../auth/models/user';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.css']
})
export class UserFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  userForm: FormGroup;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _userService: UserService,
    public dialogRef: MatDialogRef<UserFormComponent>,
    @Inject(MAT_DIALOG_DATA) public user: User,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required, Validators.maxLength(20)]],
      lastName: ['', [Validators.required, Validators.maxLength(20)]],
      email: ['', [Validators.required, Validators.email]],
      passwordHash: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(20)]],
    });
  }

  ngAfterViewInit() {
    if (this.user != null) {
      setTimeout(() => {
        this.userForm.get('email').disable();
        this.userForm.get('passwordHash').disable();
        this.userForm.patchValue(this.user);
      }, 100);
    }
  }

  onSave() {
    const user = this.userForm.value as User;
    if (this.user != null) {
      user.id = this.user.id;
      this._userService.edit(user)
        .subscribe(() => {
          this.openSnackBarBottom('Usuário editado com sucesso!', 'USUÁRIOS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar usuário: ${error.message}`, 'USUÁRIOS');
        });
    } else {
      user.userName = user.email.split('@')[0];
      this._userService.create(user)
        .subscribe(() => {
          this.openSnackBarBottom('Usuário criado com sucesso!', 'USUÁRIOS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao criar usuário: ${error.message}`, 'USUÁRIOS');
        });
    }
  }
}
