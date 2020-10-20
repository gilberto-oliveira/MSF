import { NgModule } from '@angular/core';
import { NgxMaskModule } from 'ngx-mask';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { AuthGuardService } from 'src/app/core/authentication/auth/guards/auth-guard.service';
import { SaleProcessComponent } from './sale-process/sale-process.component';
import { SaleProcessFormComponent } from './sale-process-form/sale-process-form.component';
import { SaleProcessProductComponent } from './sale-process-product/sale-process-product.component';
import { SaleProcessPaymentComponent } from './sale-process-payment/sale-process-payment.component';

const routes: Routes = [
    { path: 'process', component: SaleProcessComponent, canActivate: [AuthGuardService] },
    { path: '', redirectTo: 'process', pathMatch: 'full' },
    { path: '**', redirectTo: 'process', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        SaleProcessComponent,
        SaleProcessFormComponent,
        SaleProcessProductComponent, 
        SaleProcessPaymentComponent
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
        SaleProcessFormComponent
    ]
})
export class SaleRoutingModule { }
