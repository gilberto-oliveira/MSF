import { TestBed } from '@angular/core/testing';

import { WorkCenterControlService } from './work-center-control.service';

describe('WorkCenterControlService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkCenterControlService = TestBed.get(WorkCenterControlService);
    expect(service).toBeTruthy();
  });
});
