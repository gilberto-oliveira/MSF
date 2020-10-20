import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleProcessPaymentComponent } from './sale-process-payment.component';

describe('SaleProcessPaymentComponent', () => {
  let component: SaleProcessPaymentComponent;
  let fixture: ComponentFixture<SaleProcessPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleProcessPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleProcessPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
