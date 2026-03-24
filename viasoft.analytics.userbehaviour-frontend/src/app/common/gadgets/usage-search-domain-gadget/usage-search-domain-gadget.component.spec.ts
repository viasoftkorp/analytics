import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UsageSearchDomainGadgetComponent } from './usage-search-domain-gadget.component';

describe('UsageSearchDomainGadgetComponent', () => {
  let component: UsageSearchDomainGadgetComponent;
  let fixture: ComponentFixture<UsageSearchDomainGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UsageSearchDomainGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsageSearchDomainGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
