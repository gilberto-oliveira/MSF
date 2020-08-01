import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { ProductListComponent } from './product-list/product-list.component';
import { ProductFormComponent } from './product-form/product-form.component';
import { NgxMaskModule } from 'ngx-mask';
import { AuthGuardService } from 'src/app/core/authentication/auth/guards/auth-guard.service';

const routes: Routes = [
    { path: 'list', component: ProductListComponent, canActivate: [AuthGuardService] },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        ProductListComponent,
        ProductFormComponent
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
        ProductFormComponent,
    ]
})
export class ProductRoutingModule { }
