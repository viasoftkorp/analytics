import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourHistoricComponent } from './user-behaviour-historic.component';

describe('UserBehaviourHistoricComponent', () => {
  let component: UserBehaviourHistoricComponent;
  let fixture: ComponentFixture<UserBehaviourHistoricComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [UserBehaviourHistoricComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourHistoricComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
