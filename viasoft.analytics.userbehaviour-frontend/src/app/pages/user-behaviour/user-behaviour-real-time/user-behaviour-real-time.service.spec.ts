import { TestBed } from '@angular/core/testing';

import { UserBehaviourRealTimeService } from './user-behaviour-real-time.service';

describe('UserBehaviourRealTimeService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourRealTimeService = TestBed.get(UserBehaviourRealTimeService);
    expect(service).toBeTruthy();
  });
});
