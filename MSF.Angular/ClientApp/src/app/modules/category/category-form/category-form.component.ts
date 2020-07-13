import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { Component, Inject, OnInit, AfterViewInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatSnackBar, MatChipInputEvent } from '@angular/material';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { NavigationTitleService } from 'src/app/core/services/navigation-title.service';
import { Category } from './../models/category';
import { CategoryService } from './../services/category.service';
import { Subcategory } from './../models/subcategory';

@Component({
  selector: 'app-category-form',
  templateUrl: './category-form.component.html',
  styleUrls: ['./category-form.component.css']
})
export class CategoryFormComponent extends BaseComponent implements AfterViewInit, OnInit {

  categoryForm: FormGroup;
  readonly separatorKeysCodes: number[] = [ ENTER, COMMA ];
  subcategories: Subcategory[] = [];

  constructor(protected snackBar: MatSnackBar,
    protected _titleService: NavigationTitleService,
    private _categoryService: CategoryService,
    public dialogRef: MatDialogRef<CategoryFormComponent>,
    @Inject(MAT_DIALOG_DATA) public category: Category,
    private fb: FormBuilder) {
    super(snackBar, _titleService);
  }

  ngOnInit(): void {
    this.categoryForm = this.fb.group({
      code: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(4)]],
      description: ['', [Validators.required, Validators.minLength(5)]]
    });
  }

  ngAfterViewInit() {
    if (this.category != null) {
      setTimeout(() => {
        this.categoryForm.patchValue(this.category);
        this.subcategories = this.category.subcategories == null ? this.subcategories : this.clone(this.category.subcategories);
      }, 100);
    }
  }

  onSave() {
    const category = Object.assign({}, this.categoryForm.value) as Category;
    category.subcategories = this.subcategories;
    if (this.category != null) {
      category.id = this.category.id;
      this._categoryService.edit(category)
        .subscribe(() => {
          this.openSnackBarBottom('Categoria editada com sucesso!', 'CATEGORIAS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao criar categoria: ${error.message}`, 'CATEGORIAS');
        });
    } else {
      this._categoryService.create(category)
        .subscribe(() => {
          this.openSnackBarBottom('Categoria criada com sucesso!', 'CATEGORIAS');
          this.dialogRef.close(true);
        }, error => {
          this.openSnackBarTop(`Erro ao criar categoria: ${error.message}`, 'CATEGORIAS');
        });
    }
  }

  removeSubcategory(i: number): void {
    this.subcategories.splice(i, 1);
  }

  addSubcategory(event: MatChipInputEvent) {
    const input = event.input;
    const value = event.value;

    if ((value || '').trim()) {
      this.subcategories.push({ id: 0, description: value.trim() });
    }

    if (input) {
      input.value = '';
    }
  }
}
