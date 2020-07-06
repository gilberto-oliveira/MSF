import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from '../../services/navigation-title.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent extends BaseComponent implements OnInit {

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Página Inicial");
  }

}
