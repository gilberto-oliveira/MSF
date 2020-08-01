import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavigationComponent } from './core/layout/navigation/navigation.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MaterialModule } from './material.module';
import { AppRoutingModule } from './app-routing.module';
import { IndexComponent } from './core/layout/index/index.component';
import { UnauthorizedComponent } from './core/layout/unauthorized/unauthorized.component';
import { ProgressInterceptor } from './core/interceptors/progress-interceptor';
import { JwtInterceptorService } from './core/authentication/auth/interceptors/jwt-interceptor.service';
import { AboutComponent } from './core/layout/about/about.component';
import { ErrorInterceptorService } from './core/authentication/auth/interceptors/error-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    IndexComponent,
    NavigationComponent,
    UnauthorizedComponent,
    AboutComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    BrowserAnimationsModule,
    LayoutModule,
    MaterialModule,
    AppRoutingModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ProgressInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
