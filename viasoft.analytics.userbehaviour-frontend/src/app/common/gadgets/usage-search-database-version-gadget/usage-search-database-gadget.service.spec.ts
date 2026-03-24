import { TestBed } from '@angular/core/testing';

import { UsageSearchDatabaseGadgetService } from './usage-search-database-gadget.service';

describe('UsageSearchDatabaseGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UsageSearchDatabaseGadgetService = TestBed.get(UsageSearchDatabaseGadgetService);
    expect(service).toBeTruthy();
  });
});
