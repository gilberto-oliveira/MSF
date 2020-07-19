import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { WorkCenterListComponent } from './work-center-list/work-center-list.component';
import { WorkCenterFormComponent } from './work-center-form/work-center-form.component';

const routes: Routes = [
    { path: 'list', component: WorkCenterListComponent },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        WorkCenterListComponent,
        WorkCenterFormComponent
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
        WorkCenterFormComponent,
    ]
})
export class WorkCenterRoutingModule { }
