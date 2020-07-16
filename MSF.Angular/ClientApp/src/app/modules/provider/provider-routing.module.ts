import { NgModule } from '@angular/core';
import { NgxMaskModule } from 'ngx-mask';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { ProviderListComponent } from './provider-list/provider-list.component';
import { ProviderFormComponent } from './provider-form/provider-form.component';

const routes: Routes = [
    { path: 'list', component: ProviderListComponent },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        ProviderListComponent,
        ProviderFormComponent
    ],
    imports: [
    CommonModule,
        RouterModule.forChild(routes),
        NgxMaskModule.forRoot(),
        ReactiveFormsModule,
        HttpClientModule,
        MaterialModule
    ],
    exports: [RouterModule],
    entryComponents: [
        ProviderFormComponent
    ]
})
export class ProviderRoutingModule { }
