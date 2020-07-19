import { Component, OnInit, AfterViewInit, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ShopService } from './../services/shop.service';
import { Shop } from '../models/shop';

@Component({
  selector: 'app-shop-form',
  templateUrl: './shop-form.component.html',
  styleUrls: ['./shop-form.component.css']
})
export class ShopFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  shopForm: FormGroup;

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _shopService: ShopService,
    public dialogRef: MatDialogRef<ShopFormComponent>,
    @Inject(MAT_DIALOG_DATA) public shop: Shop,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.shopForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(10)]],
      description: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(30)]]
    });
  }

  ngAfterViewInit() {
    if (this.shop != null) {
      setTimeout(() => {
        this.shopForm.patchValue(this.shop);
      }, 100);
    }
  }

  onSave() {
    const shop = Object.assign({}, this.shopForm.value) as Shop;
    if (this.shop != null) {
      shop.id = this.shop.id;
      this._shopService.edit(shop)
        .subscribe(() => {
          this.openSnackBarBottom('Loja editada com sucesso!', 'LOJAS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao editar loja: ${error.message}`, 'LOJAS');
        });
    } else {
      this._shopService.create(shop)
        .subscribe(() => {
          this.openSnackBarBottom('Loja criada com sucesso!', 'LOJAS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao criar loja: ${error.message}`, 'LOJAS');
        });
    }
  }
}