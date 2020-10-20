import { Component, OnInit } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar, MatSelectChange, MatDialog } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ShopService } from '../../shop/services/shop.service';
import { Observable } from 'rxjs';
import { Shop } from 'src/app/modules/shop/models/shop';
import { WorkCenterService } from './../../work-center/services/work-center.service';
import { WorkCenter } from './../../work-center/models/work-center';
import { WorkCenterControlService } from '../services/work-center-control.service';
import { WorkCenterControl } from './../models/work-center-control';

@Component({
  selector: 'app-sale-process',
  templateUrl: './sale-process.component.html',
  styleUrls: ['./sale-process.component.css']
})
export class SaleProcessComponent extends BaseComponent implements OnInit {

  isLinear = false;
  saleProcessForm: FormGroup;

  workCenters: Observable<WorkCenter[]>;
  shops: Observable<Shop[]>;
  workCenterControl: WorkCenterControl;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _shopService: ShopService,
    private _workCenterService: WorkCenterService,
    private _workCenterControlService: WorkCenterControlService,
    public dialog: MatDialog,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this._titleService.setTitle('Vendas');

    this.saleProcessForm = this.fb.group({
      shopId: ['', [Validators.required]],
      workCenterId: ['', [Validators.required]]
    });

    this.getShops();
  }

  startWorkCenter() {
    if (this.workCenterControl) {
      const id = this.workCenterControl.workCenterId;
      this._workCenterService.start(id)
        .subscribe(() => {
          this.openSnackBarBottom('Caixa aberto com sucesso!', 'VENDAS');
        }, error => {
          this.openSnackBarTop(`Erro ao abrir caixa: ${error.message}`, 'VENDAS');
        });
    }
  }

  closeWorkCenter() {
    if (this.workCenterControl) {
      const id = this.workCenterControl.workCenterId;
      this._workCenterService.close(id)
        .subscribe(() => {
          this.openSnackBarBottom('Caixa fechado com sucesso!', 'VENDAS');
        }, error => {
          this.openSnackBarTop(`Erro ao fechar caixa: ${error.message}`, 'VENDAS');
        });
    }
  }

  getShops() {
    this.shops = this._shopService.findByCurrentUser();
  }

  getWorkCenters(event: MatSelectChange) {
    const shopId = event.value;
    this.workCenters = this._workCenterService.findByShop(shopId);
  }

  showWorkCenterStatus(event: MatSelectChange) {
    const workCenter = event.value as WorkCenter;
    this._workCenterControlService.lazyOpenedByWorkCenterId(workCenter.id)
      .subscribe((wcc) => {
        this.workCenterControl = wcc;
      });
  }
}
