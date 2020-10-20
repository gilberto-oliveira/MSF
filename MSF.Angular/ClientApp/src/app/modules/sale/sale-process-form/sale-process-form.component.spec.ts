import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SaleProcessFormComponent } from './sale-process-form.component';

describe('SaleProcessFormComponent', () => {
  let component: SaleProcessFormComponent;
  let fixture: ComponentFixture<SaleProcessFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SaleProcessFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SaleProcessFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
