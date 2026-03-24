import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourOnlineAppsGadgetComponent } from './user-behaviour-online-apps-gadget.component';

describe('UserBehaviourOnlineAppsGadgetComponent', () => {
  let component: UserBehaviourOnlineAppsGadgetComponent;
  let fixture: ComponentFixture<UserBehaviourOnlineAppsGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourOnlineAppsGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourOnlineAppsGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
