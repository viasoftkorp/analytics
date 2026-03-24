import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, OnDestroy } from '@angular/core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { DataLivelyService, VsSubscriptionManager } from '@viasoft/common';
import { ensureTrailingSlash } from '@viasoft/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { catchError, first, tap } from 'rxjs/operators';

import {
  LicenseUsageHistoryIndexOutput,
} from './user-behaviour-historic-index-settings/tokens/license-usage-history-index-output.token';
import {
  LicenseUsageHistoryIndexUpdated,
} from './user-behaviour-historic-index-settings/tokens/license-usage-history-index-updated.class';

@Injectable()
export class UserBehaviourIndexService implements OnDestroy {
  private subs = new VsSubscriptionManager();
  private readonly HISTORIC_INDEX_NOTIFICATION_NAME = 'LicenseUsageHistoryIndexUpdated';
  private isStarted = false;
  public historicIndex: LicenseUsageHistoryIndexUpdated;
  public historicIndexSubject = new BehaviorSubject<LicenseUsageHistoryIndexUpdated>(undefined);
  public hasLoadedIndex: boolean;
  public hasLoadedIndexSubject = new BehaviorSubject<boolean>(false);

  constructor(
    @Inject(VS_BACKEND_URL) private backendUrl: string,
    private httpClient: HttpClient,
    private dataLivelyService: DataLivelyService
  ) { }

  public start(): void {
    if (this.isStarted) {
      return;
    }
    this.isStarted = true;
    this.getCurrentIndex();
    this.getHistoricIndexUpdatedNotification();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public getCurrentIndex(): void {
    this.httpClient.get<LicenseUsageHistoryIndexOutput>(
      `${ensureTrailingSlash(this.backendUrl)}api/Analytics/UserBehaviour/LicenseUsageHistoryIndex/Get`
    ).pipe(
      first(),
      tap(((result) => {
        this.updateHistoricIndex(new LicenseUsageHistoryIndexUpdated({
          isIndexing: result?.isIndexing,
          hasEsFailedSinceLastReindex: result?.hasEsFailedSinceLastReindex,
          lastModificationTime: result?.lastModificationTime
        }));
        this.hasLoadedIndex = true;
        this.hasLoadedIndexSubject.next(true);
      }))
    ).subscribe();
  }

  public reindex(): Observable<void> {
    this.historicIndex.isIndexing = true;
    this.updateHistoricIndex();
    return this.httpClient.post<void>(
      `${ensureTrailingSlash(this.backendUrl)}api/Analytics/UserBehaviour/LicenseUsageHistoryIndex/Reindex`,
      null
    ).pipe(catchError((e) => {
      this.historicIndex.isIndexing = false;
      this.updateHistoricIndex();
      throw e;
    }));
  }

  private getHistoricIndexUpdatedNotification(): void {
    this.subs.add(
      'license-usage-history-lively',
      this.dataLivelyService.get<LicenseUsageHistoryIndexUpdated>(this.HISTORIC_INDEX_NOTIFICATION_NAME)
        .subscribe((result) => {
          this.updateHistoricIndex(result);
        })
    );
  }

  private updateHistoricIndex(value: LicenseUsageHistoryIndexUpdated = this.historicIndex): void {
    this.historicIndex = value;
    this.historicIndexSubject.next(value);
  }
}
