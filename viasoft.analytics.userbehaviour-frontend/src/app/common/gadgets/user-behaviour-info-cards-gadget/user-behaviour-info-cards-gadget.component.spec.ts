import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourInfoCardsGadgetComponent } from './user-behaviour-info-cards-gadget.component';

describe('UserBehaviourInfoCardsGadgetComponent', () => {
  let component: UserBehaviourInfoCardsGadgetComponent;
  let fixture: ComponentFixture<UserBehaviourInfoCardsGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviourInfoCardsGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourInfoCardsGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
