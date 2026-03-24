import { Injectable } from '@angular/core';
import { VsStorageService } from '@viasoft/common';
import { Subject } from 'rxjs';
import { UserBehaviourAnalyticsServiceProxy } from 'src/clients/user-behaviour';

const localStorageFilterName = 'license_usage';
@Injectable()
export class UserBehaviourLicenseUsageByIdentifierGadgetService {
  advancedFilter: string = null;
  localStorageFilters: { interval: number, advancedFilter: string } = { interval: 5, advancedFilter: null };
  advancedFilterChangedSubject: Subject<void> = new Subject();

  constructor(
    private readonly storageService: VsStorageService,
    private readonly userBehaviourAnalyticsServiceProxy: UserBehaviourAnalyticsServiceProxy
  ) {
    this.normalizeLocalStorage();
  }

  protected normalizeLocalStorage(): void {
    const oldFilterCache = localStorage.getItem(localStorageFilterName);
    const newFilterCache = this.storageService.get(localStorageFilterName);
    if (oldFilterCache && !newFilterCache) {
      this.storageService.set(localStorageFilterName, oldFilterCache);
    }
  }

  public getUsageCount(advancedFilter?: string) {
    return this.userBehaviourAnalyticsServiceProxy.countLicensesInUsageByIdentifier(advancedFilter);
  }

  public setLocalStorage(localStorageFilters: { interval: number, advancedFilter: string }) {
    this.localStorageFilters = localStorageFilters;
    this.localStorageFilters.advancedFilter = this.advancedFilter;
    this.storageService.set(localStorageFilterName, JSON.stringify(localStorageFilters));
  }

  public getLocalStorage() {
    const storage = this.storageService.get(localStorageFilterName);
    this.localStorageFilters = storage ? JSON.parse(storage) : null;
    this.advancedFilter = this.localStorageFilters.advancedFilter;
    return this.localStorageFilters;
  }

  public advancedFilterHasChanged(): void {
    this.localStorageFilters.advancedFilter = this.advancedFilter;
    this.storageService.set(localStorageFilterName, JSON.stringify(this.localStorageFilters));
    this.advancedFilterChangedSubject.next();
  }
}