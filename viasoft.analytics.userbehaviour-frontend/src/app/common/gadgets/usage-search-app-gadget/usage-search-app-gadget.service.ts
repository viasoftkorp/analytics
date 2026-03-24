import { Injectable } from '@angular/core';
import { VsStorageService } from '@viasoft/common';
import { UsageSearchServiceProxy } from 'src/clients/user-behaviour-analytics';

import { UsageSearchBaseGadgetService } from '../usage-search-base-gadget/usage-search-base-gadget.service';

@Injectable()
export class UsageSearchAppGadgetService extends UsageSearchBaseGadgetService {
  protected override filterCacheKey = 'UsageSearchAppGadgetFilter';
  constructor(storageService: VsStorageService, usageSearchServiceProxy: UsageSearchServiceProxy) {
    super(storageService, usageSearchServiceProxy, 'App');
  }
}
