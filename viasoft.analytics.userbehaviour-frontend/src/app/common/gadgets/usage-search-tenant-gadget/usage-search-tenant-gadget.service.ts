import { Injectable } from '@angular/core';
import { VsStorageService } from '@viasoft/common';
import { UsageSearchServiceProxy } from 'src/clients/user-behaviour-analytics';

import { UsageSearchBaseGadgetService } from '../usage-search-base-gadget/usage-search-base-gadget.service';

@Injectable()
export class UsageSearchTenantGadgetService extends UsageSearchBaseGadgetService {
  protected override filterCacheKey = 'UsageSearchTenantGadgetFilter';
  constructor(storageService: VsStorageService, usageSearchServiceProxy: UsageSearchServiceProxy) {
    super(storageService, usageSearchServiceProxy, 'Tenant');
  }
}
