import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Routes, RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from './../../material.module';
import { AuthGuardService } from 'src/app/core/authentication/auth/guards/auth-guard.service';
import { DashboardIndexComponent } from './dashboard-index/dashboard-index.component';
import { ChartModule } from 'angular-highcharts';

const routes: Routes = [
    { path: 'index', component: DashboardIndexComponent, canActivate: [AuthGuardService] },
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: '**', redirectTo: 'index', pathMatch: 'full' }
];

@NgModule({
    declarations: [
        DashboardIndexComponent
    ],
    imports: [
        CommonModule,
        RouterModule.forChild(routes),
        HttpClientModule,
        MaterialModule,
        ChartModule
    ],
    exports: [RouterModule],
})
export class DashboardRoutingModule { }
