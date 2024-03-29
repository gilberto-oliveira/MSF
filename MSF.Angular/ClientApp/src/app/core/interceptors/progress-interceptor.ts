import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { LoadingService } from '../services/loading.service';
​
@Injectable({
    providedIn: 'root'
})
export class ProgressInterceptor implements HttpInterceptor {
​
    constructor(private loadingService: LoadingService) { }
​
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.showLoader();
        return next.handle(req).pipe(
            tap((event: HttpEvent<any>) => {
                if (event instanceof HttpResponse) {
                    this.onEnd();
                }
            },
            (err: any) => {
                this.onEnd();
            }));
    }
​
    private onEnd(): void {
        this.hideLoader();
    }
​
    private showLoader(): void {
        this.loadingService.show();
    }
​
    private hideLoader(): void {
        this.loadingService.hide();
    }
}