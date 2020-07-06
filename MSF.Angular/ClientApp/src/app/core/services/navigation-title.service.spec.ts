/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { NavigationTitleService } from './navigation-title.service';

describe('Service: GlobalTitle', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NavigationTitleService]
    });
  });

  it('should ...', inject([NavigationTitleService], (service: NavigationTitleService) => {
    expect(service).toBeTruthy();
  }));
});
