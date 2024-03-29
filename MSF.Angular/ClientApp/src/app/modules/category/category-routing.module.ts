import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryFormComponent } from './category-form/category-form.component';
import { MaterialModule } from './../../material.module';
import { AuthGuardService } from 'src/app/core/authentication/auth/guards/auth-guard.service';

const routes: Routes = [
    { path: 'list', component: CategoryListComponent, canActivate: [AuthGuardService] },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        CategoryListComponent,
        CategoryFormComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        ReactiveFormsModule,
        HttpClientModule,
        MaterialModule
    ],
    exports: [RouterModule],
    entryComponents: [
        CategoryFormComponent,
    ]
})
export class CategoryRoutingModule { }
