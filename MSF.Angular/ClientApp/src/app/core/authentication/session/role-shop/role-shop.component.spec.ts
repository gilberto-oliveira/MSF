import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleShopComponent } from './role-shop.component';

describe('RoleShopComponent', () => {
  let component: RoleShopComponent;
  let fixture: ComponentFixture<RoleShopComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RoleShopComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RoleShopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
