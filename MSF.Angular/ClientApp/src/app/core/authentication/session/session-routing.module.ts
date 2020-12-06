import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './login/login.component';
import { MaterialModule } from './../../../material.module';
import { UserListComponent } from './user-list/user-list.component';
import { AuthGuardService } from '../auth/guards/auth-guard.service';
import { UserFormComponent } from './user-form/user-form.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleFormComponent } from './role-form/role-form.component';
import { RoleShopComponent } from './role-shop/role-shop.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'list', component: UserListComponent, canActivate: [AuthGuardService], data: { permittedRoles: ['Admin'] } },
  { path: 'role/:id', component: RoleListComponent, canActivate: [AuthGuardService], data: { permittedRoles: ['Admin'] } },
  { path: '', redirectTo: 'login', pathMatch: 'full' }
];

@NgModule({
  declarations: [
    LoginComponent,
    UserFormComponent,
    UserListComponent,
    RoleListComponent,
    RoleFormComponent,
    RoleShopComponent
  ],
  imports: [
  CommonModule,
    MaterialModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
    HttpClientModule
  ],
  entryComponents: [
    UserFormComponent,
    RoleFormComponent,
    RoleShopComponent
  ],
  exports: [RouterModule]
})
export class SessionRoutingModule { }