import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UserBehaviouLicenseUsageByIdentifierGadgetComponent } from './user-behaviour-license-usage-by-identifier.component';

describe('UserBehaviouLicenseUsageByIdentifierComponent', () => {
  let component: UserBehaviouLicenseUsageByIdentifierGadgetComponent;
  let fixture: ComponentFixture<UserBehaviouLicenseUsageByIdentifierGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UserBehaviouLicenseUsageByIdentifierGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserBehaviouLicenseUsageByIdentifierGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
