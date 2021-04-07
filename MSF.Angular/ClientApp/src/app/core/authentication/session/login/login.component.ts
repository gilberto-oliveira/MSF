import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialog, MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { AuthenticationService } from '../../auth/services/authentication.service';
import { AuthService } from "angularx-social-login";
import { GoogleLoginProvider } from "angularx-social-login";
import { UserFormComponent } from './../user-form/user-form.component';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent extends BaseComponent implements OnInit {

  authForm: FormGroup;
  returnUrl: string;

  constructor(private fb: FormBuilder,
    private router: Router,
    protected _snackBar: MatSnackBar,
    private _authService: AuthenticationService,
    private route: ActivatedRoute,
    protected _titleService: NavigationTitleService,
    private _oAuthService: AuthService,
    public dialog: MatDialog) {
      super(_snackBar, _titleService);
      if (this._authService.currentUserValue) {
        this.router.navigate(['/index']);
      }
  }

  openDialog(user: any = null) {
    const dialogRef = this.dialog.open(UserFormComponent, {
      disableClose: true,
      data: user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);
      }
    });
  }

  ngOnInit() {
    this._titleService.setTitle("Vamos começar? Faça seu login");
    this.authForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]],
      PasswordHash: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  onSubmit() {
    const auth = this.authForm.value;
    this._authService.login(auth.Email, auth.PasswordHash)
      .subscribe(_ => {
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/index';
        this.router.navigate([this.returnUrl]);
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'LOGIN');
      });
  }

  signInWithGoogle(): void {
    this._oAuthService.signIn(GoogleLoginProvider.PROVIDER_ID).then(user => {
      this.loginWithOAuth(user.firstName, user.lastName, user.email, user.idToken);
    });
  }

  private loginWithOAuth(firstName: string, lastName: string, email: string, password: string) {
    this._authService.loginWithOAuth(firstName, lastName, email, password)
      .subscribe(_ => {
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/index';
        this.router.navigate([this.returnUrl]);
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'LOGIN');
      });
  }

  signOut(): void {
    this._oAuthService.signOut();
  }

  ngOnDestroy() {
  }
}
