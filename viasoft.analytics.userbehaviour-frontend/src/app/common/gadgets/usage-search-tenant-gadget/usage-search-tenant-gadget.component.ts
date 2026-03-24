import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { UserBehaviourIndexService } from 'src/app/pages/user-behaviour/user-behaviour-index.service';
import { FilterByGroupingOutput } from 'src/clients/user-behaviour-analytics';

import { UsageSearchAppGadgetService } from '../usage-search-app-gadget/usage-search-app-gadget.service';
import { UsageSearchBaseGadget } from '../usage-search-base-gadget/usage-search-base-gadget.class';
import { UsageSearchDomainGadgetService } from '../usage-search-domain-gadget/usage-search-domain-gadget.service';
import { UsageSearchTenantGadgetService } from './usage-search-tenant-gadget.service';
import { FormBuilder } from '@angular/forms';
import { VsDialog } from '@viasoft/components';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './usage-search-tenant-gadget.component.html',
  styleUrls: ['./usage-search-tenant-gadget.component.scss']
})
export class UsageSearchTenantGadgetComponent extends UsageSearchBaseGadget implements OnInit, OnDestroy {
  constructor(
    injector: Injector,
    translateService: TranslateService,
    private readonly usageSearchTenantGadgetService: UsageSearchTenantGadgetService,
    private readonly usageSearchAppGadgetService: UsageSearchAppGadgetService,
    private readonly usageSearchDomainGadgetService: UsageSearchDomainGadgetService,
    formBuilder: FormBuilder,
    dialog: VsDialog,
    public userBehaviourIndexService: UserBehaviourIndexService
  ) {
    super(
      injector,
      translateService,
      usageSearchTenantGadgetService,
      formBuilder,
      dialog
    );
  }

  ngOnInit() {
    this.subs.add("tenantRender", this.usageSearchTenantGadgetService.renderObservable.subscribe(
      () => {
        this.chartObj.updateOptions(this.chartOptions);
      }
    ));
    this.subscribeToHistoryIndexed();
  }

  ngOnDestroy() {
    super.ngOnDestroy();
    this.usageSearchAppGadgetService.renderAgain();
    this.usageSearchDomainGadgetService.renderAgain();
  }

  getCategories(input: FilterByGroupingOutput) {
    return input.values.map(v => (v.accountName || ''));
  }

  get currentGroupingLabel(): any {
    return 'dashBoard.usageSearch.groupings.tenant.label|dateInterval:' + this.dateTimeIntervalOptions.filter(e => e.value === this.appliedDateInterval)[0].name;
  }

  protected createChart(): Subscription {
    if (this.userBehaviourIndexService?.hasLoadedIndex && !this.userBehaviourIndexService.historicIndex?.isIndexing) {
      return super.createChart();
    }
    return new Subscription();
  }

  private subscribeToHistoryIndexed(): void {
    this.subs.add("updateAfterIndexed", this.userBehaviourIndexService.historicIndexSubject
      .subscribe((index) => {
        if (index?.isIndexing) {
          return;
        }
        if (this.userBehaviourIndexService.hasLoadedIndex && !this.isChartReady) {
          this.subs.add("createAfterIndexed", this.createChart());
        } else {
          this.updateChart();
        }
      }));
  }
}
