import { NgModule, LOCALE_ID } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './core/layout/index/index.component';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { MaterialModule } from './material.module';
import { registerLocaleData } from '@angular/common';
import ptBr from '@angular/common/locales/pt';
import { UnauthorizedComponent } from './core/layout/unauthorized/unauthorized.component';
import { AuthGuardService } from './core/authentication/auth/guards/auth-guard.service';

registerLocaleData(ptBr);

const routes: Routes = [
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
      .then(m => m.CategoryModule), canActivate: [AuthGuardService]
  },
  {
    path: 'provider', loadChildren: () => import('./modules/provider/provider.module')
      .then(m => m.ProviderModule), canActivate: [AuthGuardService]
  },
  {
    path: 'shop', loadChildren: () => import('./modules/shop/shop.module')
      .then(m => m.ShopModule), canActivate: [AuthGuardService]
  },
  {
    path: 'work-center', loadChildren: () => import('./modules/work-center/work-center.module')
      .then(m => m.WorkCenterModule), canActivate: [AuthGuardService]
  },
  {
    path: 'product', loadChildren: () => import('./modules/product/product.module')
      .then(m => m.ProductModule), canActivate: [AuthGuardService]
  },
  {
    path: 'stock', loadChildren: () => import('./modules/stock/stock.module')
      .then(m => m.StockModule), canActivate: [AuthGuardService]
  }
];

@NgModule({
  declarations: [
    ConfirmDialogComponent
  ],
  exports: [RouterModule],
  imports: [
    RouterModule.forRoot(routes),
    MaterialModule
  ],
  entryComponents: [
    ConfirmDialogComponent
  ],
  providers: [{ provide: LOCALE_ID, useValue: 'pt' }]
})

export class AppRoutingModule { }
