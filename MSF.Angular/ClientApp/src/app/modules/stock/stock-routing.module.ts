import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { StockListComponent } from './stock-list/stock-list.component';
import { StockFormComponent } from './stock-form/stock-form.component';
import { NgxMaskModule } from 'ngx-mask';
import { AuthGuardService } from 'src/app/core/authentication/auth/guards/auth-guard.service';

const routes: Routes = [
    { path: 'list', component: StockListComponent, canActivate: [AuthGuardService] },
    { path: '', redirectTo: 'list', pathMatch: 'full' },
    { path: '**', redirectTo: 'list', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        StockListComponent,
        StockFormComponent
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
        StockFormComponent,
    ]
})
export class StockRoutingModule { }
