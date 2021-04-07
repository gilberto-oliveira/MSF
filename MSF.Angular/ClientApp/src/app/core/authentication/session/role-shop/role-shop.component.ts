import { Component, OnInit, Inject } from '@angular/core';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { ShopService } from 'src/app/modules/shop/services/shop.service';
import { FormBuilder } from '@angular/forms';
import { Shop } from 'src/app/modules/shop/models/shop';
import { RoleService } from './../services/role.service';

@Component({
  selector: 'app-role-shop',
  templateUrl: './role-shop.component.html',
  styleUrls: ['./role-shop.component.css']
})
export class RoleShopComponent extends BaseComponent implements OnInit {

  shops: Shop[];

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _shopService: ShopService,
    private _roleService: RoleService,
    public dialogRef: MatDialogRef<RoleShopComponent>,
    @Inject(MAT_DIALOG_DATA) public userRole: any,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit() {
    this.findShopsByUserRole();
  }

  findShopsByUserRole() {
    this._shopService.findByUserRole(this.userRole.userId, this.userRole.roleId)
      .subscribe(data => {
        this.shops = data;
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'ASSOCIAR LOJA');
      });
  }

  change(status: boolean, shopId: number) {
    if (status) {
      this._roleService.createUserRoleShop(this.userRole.userId, this.userRole.roleId, shopId)
        .subscribe(() => {
          this.openSnackBarBottom('Loja associada com sucesso', 'ASSOCIAR LOJA');
          this.findShopsByUserRole();
        }, error => {
          this.openSnackBarTop(`${error.detail}`, 'ASSOCIAR LOJA');
        });
    } else {
      this._roleService.deleteUserRoleShop(this.userRole.userId, this.userRole.roleId, shopId)
        .subscribe(() => {
          this.openSnackBarBottom('Loja desassociada com sucesso', 'ASSOCIAR LOJA');
          this.findShopsByUserRole();
        }, error => {
          this.openSnackBarTop(`${error.detail}`, 'ASSOCIAR LOJA');
        });
    }
  }

}
