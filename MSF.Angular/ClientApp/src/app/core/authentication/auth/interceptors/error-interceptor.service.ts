import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';
import { AuthenticationService } from './../services/authentication.service';
import { MatSnackBar } from '@angular/material';
import { Router } from '@angular/router';
import { ProblemDetail } from 'src/app/shared/models/problem-detail';

@Injectable()
export class ErrorInterceptorService implements HttpInterceptor {

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              protected snackBar: MatSnackBar ) { }
              
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err: HttpErrorResponse)  => {
        const problem = new ProblemDetail();
        if (err.error !== undefined) {
          switch (err.status) {
            case 0:
              Object.assign(problem,
                { title: 'Conexão Recusada', status: err.status, detail: 'Não foi possível conectar ao servidor.' });
              break;

            case 400:
              Object.assign(problem, err.error);
              problem.title = 'Um ou mais campos tem erros';
              problem.detail = problem.errors !== undefined ? JSON.stringify(problem.errors) : problem.title;
              break;

            case 401:
                this.openSnackBarFull('Sessão expirada!', 'LOGIN', 8000, 'bottom');
                this.authenticationService.logout();
                this.router.navigate(['/user/login']);
              break;

            case 403:
              Object.assign(problem,
                { title: 'Não Autorizado', status: err.status, detail: 'Você não é autorizado a realizar esta operação.' });
              break;

            case 404:
              Object.assign(problem,
                { title: 'Não Encontrado', status: err.status, detail: 'O recurso solicitado não existe.' });
              break;

            case 500:
              Object.assign(problem, err.error);
              problem.title = 'Ocorreu um erro';
              problem.detail = `Detalhes: ${problem.detail}`;
              break;

            default:
              Object.assign(problem,
                { title: err.name, status: err.status, detail: err.message });
              break;
          }
        } else {
          Object.assign(problem, { title: err.name, status: err.status, detail: err.message });
        }
        return throwError(problem);
    }));
  }

  public openSnackBarFull(message: string, action: string, duration: number, verticalPosition: any) {
    this.snackBar.open(message, action, {
      duration: duration,
      verticalPosition: verticalPosition
    });
  }
}
