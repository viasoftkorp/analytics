import { TestBed } from '@angular/core/testing';

import { UserBehaviourOnlineAppsGadgetService } from './user-behaviour-online-apps-gadget.service';

describe('UserBehaviourOnlineAppsGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourOnlineAppsGadgetService = TestBed.get(UserBehaviourOnlineAppsGadgetService);
    expect(service).toBeTruthy();
  });
});
