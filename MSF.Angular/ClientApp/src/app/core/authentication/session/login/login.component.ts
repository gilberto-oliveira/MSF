import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';

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
    protected _titleService: NavigationTitleService) {
      super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Vamos começar? Faça seu login");
    this.authForm = this.fb.group({
      Email: ['', [Validators.required, Validators.email]],
      PasswordHash: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  onSubmit() {
  }

  ngOnDestroy() {
  }
}
