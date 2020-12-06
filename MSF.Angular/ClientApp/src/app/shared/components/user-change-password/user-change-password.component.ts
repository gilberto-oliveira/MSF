import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MatSnackBar, MAT_DIALOG_DATA } from '@angular/material';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from 'src/app/core/authentication/auth/models/user';
import { AuthenticationService } from 'src/app/core/authentication/auth/services/authentication.service';
import { UserService } from 'src/app/core/authentication/session/services/user.service';
import { ConfirmPasswordValidator } from 'src/app/core/authentication/validators/ConfirmPasswordValidator';
import { PasswordValidator } from 'src/app/core/authentication/validators/PasswordValidator';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { BaseComponent } from 'src/app/shared/components/base-component';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './user-change-password.component.html',
  styleUrls: ['./user-change-password.component.css']
})
export class UserChangePasswordComponent extends BaseComponent implements OnInit {

  userForm: FormGroup;

  public currentUser: User;
  public email: string = null;

  helper = new JwtHelperService();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _userService: UserService,
    public dialogRef: MatDialogRef<UserChangePasswordComponent>,
    @Inject(MAT_DIALOG_DATA) public user: User,
    private authenticationService: AuthenticationService,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this.getCurrentUser();
  }
  
  private createForm() {
    this.userForm = this.fb.group({
      email: [this.email, [Validators.required, Validators.email]],
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required]],
      confirmPassword: ['', [Validators.required]]
    }, { validator: [ConfirmPasswordValidator.MatchPassword, PasswordValidator.Validate] } );
  }

  private getCurrentUser() {
    this.authenticationService.currentUser.subscribe(x => {
      this.currentUser = x;
      if (this.currentUser && this.currentUser.token) {
        this.email = (this.helper.decodeToken(this.currentUser.token).email as string).trim();
      }
      this.createForm();
    });
  }

  public onSave() {
    if (this.userForm.valid) {
      const user = Object.assign({}, this.userForm.value);
      this._userService.changePassword(user)
        .subscribe(_ => {
          this.openSnackBarBottom('Senha Alterada com Sucesso!', 'USUÁRIOS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao Alterar Senha: ${error.message}`, 'USUÁRIOS');
        });
    }
  }

}
