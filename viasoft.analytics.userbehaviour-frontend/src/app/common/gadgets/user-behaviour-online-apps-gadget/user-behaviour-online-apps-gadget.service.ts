import { Injectable } from '@angular/core';
import { UserBehaviourAnalyticsServiceProxy } from 'src/clients/user-behaviour-analytics';
import { OnlineAppsCountOutput } from 'src/clients/user-behaviour-analytics/model/onlineAppsCountOutput';
import { Observable, Subject } from 'rxjs';

@Injectable()
export class UserBehaviourOnlineAppsGadgetService {

  advancedFilter: string;
  advancedFilterChangedSubject: Subject<void> = new Subject();

  constructor(private readonly userBehaviourAnalyticsServiceProxy: UserBehaviourAnalyticsServiceProxy) {}

  countAllOnlineApps(): Observable<OnlineAppsCountOutput> {
    return this.userBehaviourAnalyticsServiceProxy.countAllOnlineApps(this.advancedFilter);
  }

  advancedFilterHasChanged(): void {
    this.advancedFilterChangedSubject.next();
  }
}
