import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UsageSearchTenantGadgetComponent } from './usage-search-tenant-gadget.component';

describe('UsageSearchTenantGadgetComponent', () => {
  let component: UsageSearchTenantGadgetComponent;
  let fixture: ComponentFixture<UsageSearchTenantGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UsageSearchTenantGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsageSearchTenantGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
