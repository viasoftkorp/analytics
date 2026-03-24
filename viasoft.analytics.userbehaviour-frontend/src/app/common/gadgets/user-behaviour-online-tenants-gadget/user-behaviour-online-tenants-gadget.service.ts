import { Injectable } from '@angular/core';
import { UserBehaviourAnalyticsServiceProxy } from 'src/clients/user-behaviour-analytics';

@Injectable()
export class UserBehaviourOnlineTenantsGadgetService {

  constructor(private readonly userBehaviourAnalyticsServiceProxy: UserBehaviourAnalyticsServiceProxy) { }

  countAllOnlineTenants() {
    return this.userBehaviourAnalyticsServiceProxy.countAllOnlineTenants();
  }

}
