import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { Chart } from 'angular-highcharts';
import { of as observableOf } from 'rxjs';
import * as Highcharts from 'highcharts';
import { catchError } from 'rxjs/operators';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { ProductStats } from '../models/product_stats';
import { ProductService } from './../../product/services/product.service';
import { WorkCenterService } from './../../work-center/services/work-center.service';
import { OperationService } from './../../sale/services/operation.service';

@Component({
  selector: 'app-dashboard-index',
  templateUrl: './dashboard-index.component.html',
  styleUrls: ['./dashboard-index.component.css']
})
export class DashboardIndexComponent extends BaseComponent implements OnInit {

  displayedColumns: string[] = ['sale', 'salePercent', 'profit'];
  productStats: ProductStats[];
  pageSize = 10;

  chart = new Chart({
    chart: {
      type: 'bar'
    },
    title: {
      text: 'Vendas por Produto'
    },
    legend: {
      enabled: false
    },
    xAxis: {
      type: 'category'
    },
    yAxis: {  
      title: {
        text: 'R$'
      }
    },
    plotOptions: {
      area: {
        fillOpacity: 0.5
      }
    },
    credits: {
      enabled: false
    },
  });

  salesPerWorkCenterChart = new Chart({
    chart: {
      type: 'bar'
    },
    title: {
      text: 'Vendas por Caixa'
    },
    legend: {
      enabled: false
    },
    xAxis: {
      type: 'category'
    },
    yAxis: {  
      title: {
        text: 'R$'
      }
    },
    plotOptions: {
      area: {
        fillOpacity: 0.5
      }
    },
    credits: {
      enabled: false
    },
  });

  salesPerUserChart = new Chart({
    chart: {
      type: 'bar'
    },
    title: {
      text: 'Vendas por Usuário'
    },
    legend: {
      enabled: false
    },
    xAxis: {
      type: 'category'
    },
    yAxis: {  
      title: {
        text: 'R$'
      }
    },
    plotOptions: {
      area: {
        fillOpacity: 0.5
      }
    },
    credits: {
      enabled: false
    },
  });

  salesPerCategoryChart = new Chart({
    chart: {
      type: 'bar'
    },
    title: {
      text: 'Vendas por Categoria'
    },
    legend: {
      enabled: false
    },
    xAxis: {
      type: 'category'
    },
    yAxis: {  
      title: {
        text: 'R$'
      }
    },
    plotOptions: {
      area: {
        fillOpacity: 0.5
      }
    },
    credits: {
      enabled: false
    },
  });

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _workCenterService: WorkCenterService,
    private _productService: ProductService,
    private _operationService: OperationService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle('Dashboards');
    this.getLazy('', this.pageSize);
  }
  
  getLazy(filter: string, take: number, skip: number = 0) {
    this._productService.getLazyStats(filter, take, skip)
      .subscribe(data => {
        var profit = 0, sale = 0, salePercent = 0;
        data.productStats.forEach((p, _) => {
          profit      += p.profit;
          sale        += p.sale;
          salePercent += p.salePercent;
        });
        data.productStats.forEach((p, _) => {
          const series: Highcharts.SeriesOptionsType = {
            type: 'bar',
            name: p.productName,
            data: [[p.productName, +p.sale.toFixed(2)]],
            dataLabels: {
              enabled: true
            }
          };
          this.chart.addSeries(series, true, true);
        });
        const p = new ProductStats();
        p.profit = profit; p.sale = sale; p.salePercent = salePercent / data.productStats.length;
        this.productStats = [ p ];
      }),
      catchError(() => {
        return observableOf([]);
      });

      this._workCenterService.getStats()
      .subscribe(data => {
        data.forEach((wc, _) => {
          const series: Highcharts.SeriesOptionsType = {
            type: 'bar',
            name: wc.workCenterName,
            data: [[wc.workCenterName, +wc.sale.toFixed(2)]],
            dataLabels: {
              enabled: true
            }
          };
          this.salesPerWorkCenterChart.addSeries(series, true, true);
        });
      }),
      catchError(() => {
        return observableOf([]);
      });

      this._operationService.getSalesByUser()
        .subscribe(data => {
          data.forEach((wc, _) => {
            const series: Highcharts.SeriesOptionsType = {
              type: 'bar',
              name: wc.userName,
              data: [[wc.userName, +wc.sale.toFixed(2)]],
              dataLabels: {
                enabled: true
              }
            };
            this.salesPerUserChart.addSeries(series, true, true);
          });
        }),
        catchError(() => {
          return observableOf([]);
        });

      this._operationService.getSalesByCategory()
        .subscribe(data => {
          data.forEach((wc, _) => {
            const series: Highcharts.SeriesOptionsType = {
              type: 'bar',
              name: wc.category,
              data: [[wc.category, +wc.sale.toFixed(2)]],
              dataLabels: {
                enabled: true
              }
            };
            this.salesPerCategoryChart.addSeries(series, true, true);
          });
        }),
        catchError(() => {
          return observableOf([]);
        });
    }
}