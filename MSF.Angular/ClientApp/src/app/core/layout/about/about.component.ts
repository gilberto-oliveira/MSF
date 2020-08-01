import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from '../../services/navigation-title.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent extends BaseComponent implements OnInit {

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle("Sobre");
  }

}
