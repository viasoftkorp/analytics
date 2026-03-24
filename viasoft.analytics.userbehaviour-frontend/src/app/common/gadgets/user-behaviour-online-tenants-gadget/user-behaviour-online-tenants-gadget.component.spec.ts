import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourOnlineTenantsGadgetComponent } from './user-behaviour-online-tenants-gadget.component';

describe('UserBehaviourOnlineTenantsGadgetComponent', () => {
  let component: UserBehaviourOnlineTenantsGadgetComponent;
  let fixture: ComponentFixture<UserBehaviourOnlineTenantsGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourOnlineTenantsGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourOnlineTenantsGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
