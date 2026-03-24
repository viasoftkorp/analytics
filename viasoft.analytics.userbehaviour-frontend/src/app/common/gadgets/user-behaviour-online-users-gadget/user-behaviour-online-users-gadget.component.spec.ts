import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourOnlineUsersGadgetComponent } from './user-behaviour-online-users-gadget.component';

describe('UserBehaviourOnlineUsersGadgetComponent', () => {
  let component: UserBehaviourOnlineUsersGadgetComponent;
  let fixture: ComponentFixture<UserBehaviourOnlineUsersGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourOnlineUsersGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourOnlineUsersGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
