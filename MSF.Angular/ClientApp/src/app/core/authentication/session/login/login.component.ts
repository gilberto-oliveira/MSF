import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { AuthenticationService } from '../../auth/services/authentication.service';

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
    protected snackBar: MatSnackBar,
    private _authService: AuthenticationService,
    private route: ActivatedRoute,
    protected _titleService: NavigationTitleService) {
      super(snackBar, _titleService);
      if (this._authService.currentUserValue) {
        this.router.navigate(['/index']);
      }
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
      .subscribe(user => {
        console.log(user);
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/index';
        this.router.navigate([this.returnUrl]);
      }, error => {
        this.openSnackBarTop(`Erro: ${error.message}`, 'LOGIN');
      });
  }

  ngOnDestroy() {
  }
}
