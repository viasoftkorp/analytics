import { TestBed } from '@angular/core/testing';

import { UsageSearchTenantGadgetService } from './usage-search-tenant-gadget.service';

describe('UsageSearchTenantGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UsageSearchTenantGadgetService = TestBed.get(UsageSearchTenantGadgetService);
    expect(service).toBeTruthy();
  });
});
