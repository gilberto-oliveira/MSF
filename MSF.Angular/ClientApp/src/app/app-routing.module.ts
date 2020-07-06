import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { IndexComponent } from './core/layout/index/index.component';

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
  }
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)]
})


export class AppRoutingModule { }
