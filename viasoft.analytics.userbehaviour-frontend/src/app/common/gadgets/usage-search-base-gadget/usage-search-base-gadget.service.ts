import { VsStorageService } from '@viasoft/common';
import { Observable, Subject } from 'rxjs';
import { FilterByGroupingOutput, Groupings, UsageSearchServiceProxy } from 'src/clients/user-behaviour-analytics';

import { UsageSearchLocalStorage } from '../../classes/usage-search-local-storage.class';
import { UsageSearchFilter } from './usage-search-filter';
import { finalize } from 'rxjs/operators';

export class UsageSearchBaseGadgetService {
  public renderObservable: Subject<void> = new Subject();
  protected filterCacheKey: string;
  private advancedFilter: string = null;
  private usageSearchAdvancedFilterCacheKey: string = 'usageSearchAdvancedFilter';

  constructor(
    private readonly storageService: VsStorageService,
    private readonly usageSearchServiceProxy: UsageSearchServiceProxy,
    private grouping: Groupings
  ) {
    this.normalizeLocalStorage();
    this.retrieveInitialAdvancedFilter();
  }
  public getFilterFromCache(): UsageSearchFilter {
    const filterCache = this.storageService.get(this.filterCacheKey);
    let filter = filterCache ? JSON.parse(filterCache) as UsageSearchFilter : null;
    if (!filter) {
      filter = {
        dateInterval: 1,
        maxResultCount: 10,
      } as UsageSearchFilter;
      this.setFilterToCache(filter);
    }
    return filter;
  }
  public setFilterToCache(filter: UsageSearchFilter): void {
    this.storageService.set(this.filterCacheKey, JSON.stringify(filter));
  }

  protected normalizeLocalStorage(): void {
    const oldFilterCache = localStorage.getItem(this.usageSearchAdvancedFilterCacheKey);
    const newFilterCache = this.storageService.get(this.usageSearchAdvancedFilterCacheKey);
    if (oldFilterCache && !newFilterCache) {
      this.storageService.set(this.usageSearchAdvancedFilterCacheKey, oldFilterCache);
    }
  }

  protected retrieveInitialAdvancedFilter(): void {
    const advancedFilterCache = this.storageService.get(this.usageSearchAdvancedFilterCacheKey);
    const json = advancedFilterCache ? JSON.parse(advancedFilterCache) as UsageSearchLocalStorage : null;
    if (json) {
      this.advancedFilter = json.advancedFilter;
    }
  }

  filterByGrouping(filter: UsageSearchFilter): Observable<FilterByGroupingOutput> {
    return this.usageSearchServiceProxy.filterByGrouping(
      this.grouping,
      this.advancedFilter,
      filter
    ).pipe(finalize(() => {
      this.setFilterToCache(filter);
    }));
  }

  renderAgain() {
    this.renderObservable.next();
  }

  public updateAdvancedFilter(input: string): void {
    this.advancedFilter = input;
    this.storageService.set(
      this.usageSearchAdvancedFilterCacheKey,
      JSON.stringify({ advancedFilter: input } as UsageSearchLocalStorage)
    );
  }

  public clearAdvancedFilter(): void {
    this.advancedFilter = null;
    this.storageService.set(
      this.usageSearchAdvancedFilterCacheKey,
      JSON.stringify({ advancedFilter: null } as UsageSearchLocalStorage)
    );
  }
}
