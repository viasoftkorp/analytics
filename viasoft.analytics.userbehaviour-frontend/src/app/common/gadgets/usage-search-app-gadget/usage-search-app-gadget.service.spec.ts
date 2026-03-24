import { TestBed } from '@angular/core/testing';

import { UsageSearchAppGadgetService } from './usage-search-app-gadget.service';

describe('UsageSearchAppGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UsageSearchAppGadgetService = TestBed.get(UsageSearchAppGadgetService);
    expect(service).toBeTruthy();
  });
});
