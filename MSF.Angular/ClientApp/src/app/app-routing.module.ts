import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './core/layout/index/index.component';
import { ConfirmDialogComponent } from './shared/components/confirm-dialog/confirm-dialog.component';
import { MaterialModule } from './material.module';

const routes: Routes = [
  {
    path: 'index', component: IndexComponent
  },
  /*{
    path: 'login', component: AuthComponent
  },*/
  {
    path: '', redirectTo: '/index', pathMatch: 'full'
  },/*
  {
    path: 'equipment', component: EquipmentComponent, canActivate: [AuthGuard],
  },
  {
    path: '**', component: Error404Component
  }*/
  { 
    path: 'user', loadChildren: () => import('./core/authentication/session/session.module')
      .then(m => m.SessionModule)
  },
  {
    path: 'category', loadChildren: () => import('./modules/category/category.module')
      .then(m => m.CategoryModule)
  },
  {
    path: 'provider', loadChildren: () => import('./modules/provider/provider.module')
      .then(m => m.ProviderModule)
  },
  {
    path: 'shop', loadChildren: () => import('./modules/shop/shop.module')
      .then(m => m.ShopModule)
  },
  {
    path: 'work-center', loadChildren: () => import('./modules/work-center/work-center.module')
      .then(m => m.WorkCenterModule)
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
  ]
})

export class AppRoutingModule { }
