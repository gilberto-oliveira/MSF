import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { MatSnackBar, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { RoleService } from '../services/role.service';
import { BaseComponent } from 'src/app/shared/components/base-component';

@Component({
  selector: 'app-role-form',
  templateUrl: './role-form.component.html',
  styleUrls: ['./role-form.component.css']
})
export class RoleFormComponent extends BaseComponent implements OnInit {

  roleForm: FormGroup;
  public roles: Observable<any[]> = new Observable<any[]>();

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _roleService: RoleService,
    public dialogRef: MatDialogRef<RoleFormComponent>,
    @Inject(MAT_DIALOG_DATA) public userId: number,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.roleForm = this.fb.group({
      roleId: ['', [Validators.required]]
    });
    this.loadRolesWithoutUser();
  }

  onSave() {
    const role = this.roleForm.value;
    this._roleService.create(this.userId, role.roleId)
      .subscribe(() => {
        this.openSnackBarBottom('Perfil associado com sucesso!', 'PERFIS');
        this.dialogRef.close(true);
      }, error => {
        this.openSnackBarTop(`${error.detail}`, 'PERFIS');
      });
  }

  loadRolesWithoutUser() {
    this.roles = this._roleService.getWithoutUser(this.userId);
  }

}
