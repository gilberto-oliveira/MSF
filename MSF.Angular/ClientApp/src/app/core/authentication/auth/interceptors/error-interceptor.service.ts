import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { AuthenticationService } from './../services/authentication.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptorService implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              protected snackBar: MatSnackBar ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse) => {
        if (err.error !== undefined) {
          switch (err.status) {
            case 401:
              this.openSnackBarFull('Sessão expirada!', 'LOGIN', 8000, 'bottom');
              this.authenticationService.logout();
              this.router.navigate(['/user/login']);
              break;
          }
        }
        return throwError('');
      }));
  }

  public openSnackBarFull(message: string, action: string, duration: number, verticalPosition: any) {
    this.snackBar.open(message, action, {
      duration: duration,
      verticalPosition: verticalPosition
    });
  }
}
