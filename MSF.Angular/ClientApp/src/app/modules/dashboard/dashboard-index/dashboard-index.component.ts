import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { BaseComponent } from 'src/app/shared/components/base-component';

@Component({
  selector: 'app-dashboard-index',
  templateUrl: './dashboard-index.component.html',
  styleUrls: ['./dashboard-index.component.css']
})
export class DashboardIndexComponent extends BaseComponent implements OnInit {

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle('Dashboards');
  }
  
}
