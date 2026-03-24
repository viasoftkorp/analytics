import { TestBed } from '@angular/core/testing';

import { UsageSearchDomainGadgetService } from './usage-search-domain-gadget.service';

describe('UsageSearchDomainGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UsageSearchDomainGadgetService = TestBed.get(UsageSearchDomainGadgetService);
    expect(service).toBeTruthy();
  });
});
