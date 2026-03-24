import { TestBed } from '@angular/core/testing';

import { UserBehaviourHistoricService } from './user-behaviour-historic.service';

describe('UserBehaviourHistoricService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: UserBehaviourHistoricService = TestBed.get(UserBehaviourHistoricService);
    expect(service).toBeTruthy();
  });
});
