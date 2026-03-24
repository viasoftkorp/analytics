import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourRealTimeComponent } from './user-behaviour-real-time.component';

describe('UserBehaviourRealTimeComponent', () => {
  let component: UserBehaviourRealTimeComponent;
  let fixture: ComponentFixture<UserBehaviourRealTimeComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourRealTimeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourRealTimeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
