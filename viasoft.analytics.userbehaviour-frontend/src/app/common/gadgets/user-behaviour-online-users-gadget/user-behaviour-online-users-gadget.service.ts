import { Injectable } from '@angular/core';
import { UserBehaviourAnalyticsServiceProxy } from 'src/clients/user-behaviour-analytics';
import { OnlineUserCountOutput } from 'src/clients/user-behaviour-analytics/model/onlineUserCountOutput';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserBehaviourOnlineUsersGadgetService {

  advancedFilter: string;
  advancedFilterChangedSubject: Subject<void> = new Subject();

  constructor(private readonly userBehaviourAnalyticsServiceProxy: UserBehaviourAnalyticsServiceProxy) { }

  countAllOnlineUsers(): Observable<OnlineUserCountOutput> {
    return this.userBehaviourAnalyticsServiceProxy.countAllOnlineUsers(this.advancedFilter);
  }

  advancedFilterHasChanged(): void {
    this.advancedFilterChangedSubject.next();
  }
}
