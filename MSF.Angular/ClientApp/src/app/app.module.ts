import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
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
import { AuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';
import { SocialLoginModule } from 'angularx-social-login';

let config = new AuthServiceConfig([
  {
    id: GoogleLoginProvider.PROVIDER_ID,
    provider: new GoogleLoginProvider("719320913586-ovijs45hhc46pm2584fvnsulblf3l30h.apps.googleusercontent.com")
  }
]);
 
export function provideConfig() {
  return config;
}

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
    ReactiveFormsModule,
    BrowserAnimationsModule,
    LayoutModule,
    MaterialModule,
    AppRoutingModule,
    SocialLoginModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ProgressInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptorService, multi: true },
    { provide: AuthServiceConfig, useFactory: provideConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
