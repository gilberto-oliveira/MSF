import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { ShopListComponent } from './shop-list/shop-list.component';
import { ShopFormComponent } from './shop-form/shop-form.component';

const routes: Routes = [
    { path: 'list', component: ShopListComponent },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        ShopListComponent,
        ShopFormComponent
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
        ShopFormComponent,
    ]
})
export class ShopRoutingModule { }
