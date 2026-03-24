import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';
import { UserBehaviourIndexService } from 'src/app/pages/user-behaviour/user-behaviour-index.service';
import { FilterByGroupingOutput } from 'src/clients/user-behaviour-analytics';

import { UsageSearchBaseGadget } from '../usage-search-base-gadget/usage-search-base-gadget.class';
import { UsageSearchDomainGadgetService } from '../usage-search-domain-gadget/usage-search-domain-gadget.service';
import { UsageSearchTenantGadgetService } from '../usage-search-tenant-gadget/usage-search-tenant-gadget.service';
import { UsageSearchAppGadgetService } from './usage-search-app-gadget.service';
import { FormBuilder } from '@angular/forms';
import { VsDialog } from '@viasoft/components';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './usage-search-app-gadget.component.html',
  styleUrls: ['./usage-search-app-gadget.component.scss']
})
export class UsageSearchAppGadgetComponent extends UsageSearchBaseGadget implements OnInit, OnDestroy {
  constructor(
    injector: Injector,
    translateService: TranslateService,
    formBuilder: FormBuilder,
    dialog: VsDialog,
    private readonly usageSearchAppGadgetService: UsageSearchAppGadgetService,
    private readonly usageSearchDomainGadgetService: UsageSearchDomainGadgetService,
    private readonly usageSearchTenantGadgetService: UsageSearchTenantGadgetService,
    public userBehaviourIndexService: UserBehaviourIndexService
  ) {
    super(
      injector,
      translateService,
      usageSearchAppGadgetService,
      formBuilder,
      dialog
    );
  }

  ngOnInit(): void {
    this.subs.add("appRender", this.usageSearchAppGadgetService.renderObservable.subscribe(
      () => {
        this.chartObj.updateOptions(this.chartOptions);
      }
    ));
    this.subscribeToHistoryIndexed();
  }

  ngOnDestroy(): void {
    super.ngOnDestroy();
    this.usageSearchDomainGadgetService.renderAgain();
    this.usageSearchTenantGadgetService.renderAgain();
  }

  getCategories(input: FilterByGroupingOutput) {
    return input.values.map(v => `${v.appName} - ${v.appIdentifier}`);
  }

  get currentGroupingLabel(): any {
    return 'dashBoard.usageSearch.groupings.app.label|dateInterval:' + this.dateTimeIntervalOptions.filter(e => e.value === this.appliedDateInterval)[0].name;
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
