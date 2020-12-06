import { NgModule, LOCALE_ID } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './core/layout/index/index.component';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { MaterialModule } from './material.module';
import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { UnauthorizedComponent } from './core/layout/unauthorized/unauthorized.component';
import { AuthGuardService } from './core/authentication/auth/guards/auth-guard.service';
import { AboutComponent } from './core/layout/about/about.component';
import { UserChangePasswordComponent } from './shared/components/user-change-password/user-change-password.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

registerLocaleData(ptBr);

const routes: Routes = [
  {
    path: 'about', component: AboutComponent
  },
  {
    path: 'unauthorized', component: UnauthorizedComponent, canActivate: [AuthGuardService]
  },
  {
    path: 'index', component: IndexComponent, canActivate: [AuthGuardService]
  },
  {
    path: '', redirectTo: 'user', pathMatch: 'full'
  },/*
  {
    path: '**', component: Error404Component
  }*/
  {
    path: 'user', loadChildren: () => import('./core/authentication/session/session.module')
      .then(m => m.SessionModule)
  },
  {
    path: 'category', loadChildren: () => import('./modules/category/category.module')
      .then(m => m.CategoryModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'provider', loadChildren: () => import('./modules/provider/provider.module')
      .then(m => m.ProviderModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'shop', loadChildren: () => import('./modules/shop/shop.module')
      .then(m => m.ShopModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'work-center', loadChildren: () => import('./modules/work-center/work-center.module')
      .then(m => m.WorkCenterModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'product', loadChildren: () => import('./modules/product/product.module')
      .then(m => m.ProductModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'stock', loadChildren: () => import('./modules/stock/stock.module')
      .then(m => m.StockModule), data: { permittedRoles: ['Admin'] }
  },
  {
    path: 'sale', loadChildren: () => import('./modules/sale/sale.module')
      .then(m => m.SaleModule), data: { permittedRoles: ['Admin', 'Vendedor'] }
  },
  {
    path: 'dashboard', loadChildren: () => import('./modules/dashboard/dashboard.module')
      .then(m => m.DashboardModule), data: { permittedRoles: ['Admin', 'Vendedor'] }
  }
];

@NgModule({
  declarations: [
    ConfirmDialogComponent,
    UserChangePasswordComponent
  ],
  exports: [RouterModule],
  imports: [
    RouterModule.forRoot(routes),
    MaterialModule,
    FormsModule,
    ReactiveFormsModule
  ],
  entryComponents: [
    ConfirmDialogComponent,
    UserChangePasswordComponent
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }]
})

export class AppRoutingModule { }
