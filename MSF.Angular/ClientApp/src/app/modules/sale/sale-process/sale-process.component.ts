import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-sale-process',
  templateUrl: './sale-process.component.html',
  styleUrls: ['./sale-process.component.css']
})
export class SaleProcessComponent extends BaseComponent implements OnInit {

  isLinear = false;
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle('Vendas');

    this.firstFormGroup = this.fb.group({
      firstCtrl: ['', Validators.required]
    });
    this.secondFormGroup = this.fb.group({
      secondCtrl: ['', Validators.required]
    });
  }

}
