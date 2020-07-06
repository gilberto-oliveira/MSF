import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent extends BaseComponent implements OnInit {

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Categorias");
  }

}
