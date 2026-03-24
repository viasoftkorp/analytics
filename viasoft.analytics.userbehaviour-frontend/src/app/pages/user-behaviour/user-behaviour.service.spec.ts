import { TestBed } from '@angular/core/testing';

import { UserBehaviourService } from './user-behaviour.service';

describe('UserBehaviourService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourService = TestBed.get(UserBehaviourService);
    expect(service).toBeTruthy();
  });
});
