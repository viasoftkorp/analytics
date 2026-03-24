import { Injectable } from '@angular/core';
import {
  UserBehaviourOnlineUsersGadgetService
} from 'src/app/common/gadgets/user-behaviour-online-users-gadget/user-behaviour-online-users-gadget.service';
import {
  UserBehaviourOnlineAppsGadgetService
} from 'src/app/common/gadgets/user-behaviour-online-apps-gadget/user-behaviour-online-apps-gadget.service';
import { GetAllInput } from 'src/app/common/inputs/getAllInput';
import { UserBehaviourLicenseUsageByIdentifierGadgetService } from 'src/app/common/gadgets/user-behaviour-license-usage-by-identifier/user-behaviour-license-usage-by-identifier.service';

@Injectable()
export class UserBehaviourService {

  listOfServices: Array<any> = [
    this.userBehaviourOnlineAppsGadgetService,
    this.userBehaviourOnlineUsersGadgetService,
    this.userBehaviourLicenseUsageByIdentifierGadgetService
  ];

  constructor(private readonly userBehaviourOnlineUsersGadgetService: UserBehaviourOnlineUsersGadgetService,
              private readonly userBehaviourOnlineAppsGadgetService: UserBehaviourOnlineAppsGadgetService,
              private readonly userBehaviourLicenseUsageByIdentifierGadgetService: UserBehaviourLicenseUsageByIdentifierGadgetService) { }

  private ApplyAdvancedFilter(input: GetAllInput): void {
    this.listOfServices.forEach(s => s.advancedFilter = input.advancedFilter);
  }

  private NotifyAdvancedFilterHasChanged(): void {
    this.userBehaviourOnlineAppsGadgetService.advancedFilterHasChanged();
    this.userBehaviourOnlineUsersGadgetService.advancedFilterHasChanged();
    this.userBehaviourLicenseUsageByIdentifierGadgetService.advancedFilterHasChanged();
  }

  private ClearAdvancedFilters(): void {
    this.listOfServices.forEach(s => s.advancedFilter = null);
  }

  notifyAddSubject(input: GetAllInput) {
    this.ApplyAdvancedFilter(input);
    this.NotifyAdvancedFilterHasChanged();
  }

  notifyClearSubject() {
    this.ClearAdvancedFilters();
    this.NotifyAdvancedFilterHasChanged();
  }
}
