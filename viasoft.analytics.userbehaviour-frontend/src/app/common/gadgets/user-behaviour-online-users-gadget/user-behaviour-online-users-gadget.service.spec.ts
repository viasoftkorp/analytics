import { TestBed } from '@angular/core/testing';

import { UserBehaviourOnlineUsersGadgetService } from './user-behaviour-online-users-gadget.service';

describe('UserBehaviourOnlineUsersGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourOnlineUsersGadgetService = TestBed.get(UserBehaviourOnlineUsersGadgetService);
    expect(service).toBeTruthy();
  });
});
