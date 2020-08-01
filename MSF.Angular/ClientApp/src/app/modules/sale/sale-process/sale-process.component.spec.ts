import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleProcessComponent } from './sale-process.component';

describe('SaleProcessComponent', () => {
  let component: SaleProcessComponent;
  let fixture: ComponentFixture<SaleProcessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleProcessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleProcessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
