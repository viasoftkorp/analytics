import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { UserBehaviourIndexService } from '../user-behaviour-index.service';
import { LicenseUsageHistoryIndexOutput } from './tokens/license-usage-history-index-output.token';

@Component({
  selector: 'app-user-behaviour-historic-index-settings',
  templateUrl: './user-behaviour-historic-index-settings.component.html',
  styleUrls: ['./user-behaviour-historic-index-settings.component.scss']
})
export class UserBehaviourHistoricIndexSettingsComponent implements OnInit, OnDestroy {
  private readonly errorMessageTimeoutInterval = 5000; // 5s
  private getIndexSubscription: Subscription;
  public indexSettings = new LicenseUsageHistoryIndexOutput();
  public errorDuringRequest: boolean;

  constructor(
    public userBehaviourIndexService: UserBehaviourIndexService
  ) { }

  ngOnInit(): void {
    this.getIndexSettings();
  }

  ngOnDestroy(): void {
    if (this.getIndexSubscription) {
      this.getIndexSubscription.unsubscribe();
    }
  }

  public reindex(): void {
    this.indexSettings.isIndexing = true;
    this.userBehaviourIndexService.reindex().subscribe(
      () => { },
      (error: HttpErrorResponse) => {
        if ([404, 500, 401].includes(error.status)) {
          this.indexSettings.isIndexing = false;
          this.errorDuringRequest = true;
          setTimeout(() => {
            this.errorDuringRequest = false;
          }, this.errorMessageTimeoutInterval);
        }
      }
    );
  }

  private getIndexSettings(): void {
    this.getIndexSubscription = this.userBehaviourIndexService.historicIndexSubject.subscribe((result) => {
      this.indexSettings = new LicenseUsageHistoryIndexOutput({
        hasEsFailedSinceLastReindex: result?.hasEsFailedSinceLastReindex,
        isIndexing: result?.isIndexing,
        lastModificationTime: result?.lastModificationTime,
      });
    });
  }
}
