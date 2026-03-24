import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviourHistoricIndexSettingsComponent } from './user-behaviour-historic-index-settings.component';

describe('UserBehaviourHistoricIndexSettingsComponent', () => {
  let component: UserBehaviourHistoricIndexSettingsComponent;
  let fixture: ComponentFixture<UserBehaviourHistoricIndexSettingsComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [UserBehaviourHistoricIndexSettingsComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviourHistoricIndexSettingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
