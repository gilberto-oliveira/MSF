import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleProcessProductComponent } from './sale-process-product.component';

describe('SaleProcessProductComponent', () => {
  let component: SaleProcessProductComponent;
  let fixture: ComponentFixture<SaleProcessProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleProcessProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleProcessProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
