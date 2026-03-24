import { TestBed } from '@angular/core/testing';

import { UserBehaviourIndexService } from './user-behaviour-index.service';

describe('UserBehaviourIndexService', () => {
  let service: UserBehaviourIndexService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserBehaviourIndexService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
