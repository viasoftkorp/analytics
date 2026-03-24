import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { UsageSearchDatabaseGadgetComponent } from './usage-search-database-gadget.component';

describe('UsageSearchDatabaseGadgetComponent', () => {
  let component: UsageSearchDatabaseGadgetComponent;
  let fixture: ComponentFixture<UsageSearchDatabaseGadgetComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ UsageSearchDatabaseGadgetComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsageSearchDatabaseGadgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
