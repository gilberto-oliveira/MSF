/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { AuthGuardService } from './auth-guard.service';
import { Router } from '@angular/router';

describe('Service: AuthGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AuthGuardService]
    });
  });

  it('should ...', inject([Router, AuthGuardService], (router: Router, service: AuthGuardService) => {
    expect([router, service]).toBeTruthy();
  }));
});
