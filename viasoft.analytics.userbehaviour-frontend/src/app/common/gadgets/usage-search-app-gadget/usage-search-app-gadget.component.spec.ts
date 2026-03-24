import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UsageSearchAppGadgetComponent } from './usage-search-app-gadget.component';

describe('UsageSearchGadgetComponent', () => {
  let component: UsageSearchAppGadgetComponent;
  let fixture: ComponentFixture<UsageSearchAppGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UsageSearchAppGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsageSearchAppGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
