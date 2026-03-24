import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourDashboardComponent } from './user-behaviour-dashboard.component';

describe('UserBehaviourDashboardComponent', () => {
  let component: UserBehaviourDashboardComponent;
  let fixture: ComponentFixture<UserBehaviourDashboardComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
