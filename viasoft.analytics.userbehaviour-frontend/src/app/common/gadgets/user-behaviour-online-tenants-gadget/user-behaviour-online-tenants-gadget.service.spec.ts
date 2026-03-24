import { TestBed } from '@angular/core/testing';

import { UserBehaviourOnlineTenantsGadgetService } from './user-behaviour-online-tenants-gadget.service';

describe('UserBehaviourOnlineTenantsGadgetService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourOnlineTenantsGadgetService = TestBed.get(UserBehaviourOnlineTenantsGadgetService);
    expect(service).toBeTruthy();
  });
});
