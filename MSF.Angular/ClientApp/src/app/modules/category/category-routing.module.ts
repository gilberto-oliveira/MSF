import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryFormComponent } from './category-form/category-form.component';
import { MaterialModule } from './../../material.module';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';

const routes: Routes = [
    { path: 'list', component: CategoryListComponent },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        CategoryListComponent,
        CategoryFormComponent,
        ConfirmDialogComponent
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
        ConfirmDialogComponent
    ]
})
export class CategoryRoutingModule { }
