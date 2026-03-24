import { Component, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import {
  JQQB_OP_CONTAINS,
  JQQB_OP_EQUAL,
  JQQBRule,
  JQQBRuleSet,
  VsSubscriptionManager
} from '@viasoft/common';
import { VsFilterGetItemsInput, VsFilterGetItemsOutput, VsFilterItem } from '@viasoft/components/filter';
import {
  VsGridDateTimeColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridNumberColumn,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components/grid';
import { Observable, of } from 'rxjs';
import { filter, map } from 'rxjs/operators';
import { Domain } from 'src/app/common/enums/domains.enum';
import { UsageSearchAppGadgetService } from 'src/app/common/gadgets/usage-search-app-gadget/usage-search-app-gadget.service';
import {
  UsageSearchDomainGadgetService,
} from 'src/app/common/gadgets/usage-search-domain-gadget/usage-search-domain-gadget.service';
import {
  UsageSearchTenantGadgetService,
} from 'src/app/common/gadgets/usage-search-tenant-gadget/usage-search-tenant-gadget.service';
import { GetAllInput } from 'src/app/common/inputs/getAllInput';
import {
  LicenseUserBehaviourHistoricOutputPagedResultDto,
} from 'src/clients/user-behaviour/model/licenseUserBehaviourHistoricOutputPagedResultDto';

import { UserBehaviourIndexService } from '../user-behaviour-index.service';
import { USER_BEHAVIOUR_HISTORIC_DOMAINS } from './tokens/consts/user-behaviour-historic-domain.const';
import { UserBehaviourHistoricService } from './user-behaviour-historic.service';

@Component({
  selector: 'app-user-behaviour-historic',
  templateUrl: './user-behaviour-historic.component.html',
  styleUrls: ['./user-behaviour-historic.component.scss']
})
export class UserBehaviourHistoricComponent implements OnInit, OnDestroy {
  private hasGridStarted: boolean;
  private subs = new VsSubscriptionManager()
  public grid: VsGridOptions;

  constructor(
    private readonly translateService: TranslateService,
    private readonly userBehaviourHistoricService: UserBehaviourHistoricService,
    private readonly usageSearchAppGadgetService: UsageSearchAppGadgetService,
    private readonly usageSearchDomainGadgetService: UsageSearchDomainGadgetService,
    private readonly usageSearchTenantGadgetService: UsageSearchTenantGadgetService,
    public userBehaviourIndexService: UserBehaviourIndexService
  ) { }

  ngOnInit(): void {
    this.configureGrid();
    this.subscribeToHistoryIndexed();
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  private getTranslationForDomain(domain: string): string {
    if (!domain) {
      return domain;
    }
    return this.translateService.instant(
      `userBehaviour.domain.domains.${domain.charAt(0).toLowerCase()}${domain.substring(1)}`
    );
  }

  private configureGrid(): void {
    this.grid = new VsGridOptions();
    this.grid.id = '4f690966-fc88-46a1-8815-03a9a7e78ba6';
    this.grid.filterType = 'elastic-search-scored';

    this.grid.columns = [
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.licensingIdentifier',
        field: 'licensingIdentifier',
        disableFilter: true,
        width: 260
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.accountName',
        field: 'accountName'
      }),
      new VsGridDateTimeColumn({
        headerName: 'userBehaviour.lastUpdate',
        field: 'lastUpdate',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.domain.domain',
        field: 'domain',
        translate: true,
        width: 150,
        sorting: {
          disable: true
        },
        filterOptions: {
          mode: 'selection',
          multiple: true,
          getItemsFilterFields: ['domain'],
          getValidItems: (input: string[]) => of(input
            .filter((i) => USER_BEHAVIOUR_HISTORIC_DOMAINS.find((d) => d.key === i))
            .map((i) => (USER_BEHAVIOUR_HISTORIC_DOMAINS.find((d) => d.key === i) as any as VsFilterItem))),
          getItems: (i: VsFilterGetItemsInput) => {
            let domains = [...USER_BEHAVIOUR_HISTORIC_DOMAINS];
            if (i.filter) {
              domains = domains.filter((domain) => {
                const filter = JSON.parse(i.filter) as JQQBRuleSet;
                const domainTranslation = this.getTranslationForDomain(domain.key).toLowerCase();
                let found = false;
                if (filter.rules) {
                  filter.rules.forEach((rule: JQQBRule) => {
                    if (rule.operator === JQQB_OP_CONTAINS.operator) {
                      found = found || domainTranslation.includes(rule.value.toLowerCase());
                    } else if (rule.operator === JQQB_OP_EQUAL.operator) {
                      found = found || domainTranslation === rule.value.toLowerCase();
                    }
                  });
                }
                return found;
              });
            }
            return of(<VsFilterGetItemsOutput>{
              items: domains
                .slice(i.skipCount, i.skipCount + i.maxResultCount)
                .map((domain) => ({
                  key: domain.value,
                  value: this.getTranslationForDomain(domain.key)
                } as any as VsFilterItem)),
              totalCount: USER_BEHAVIOUR_HISTORIC_DOMAINS.length
            });
          }
        },
        format: (value) => `userBehaviour.domain.domains.${Domain[value]}`
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.databaseName',
        field: 'databaseName',
        width: 260
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.user',
        field: 'user',
        width: 150
      }),
      new VsGridDateTimeColumn({
        headerName: 'userBehaviour.startTime',
        field: 'startTime',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.accessDuration',
        field: 'accessDurationFormatted',
        disableFilter: true,
        sorting: {
          disable: true
        },
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.appIdentifier',
        field: 'appIdentifier',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.appName',
        field: 'appName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.bundleIdentifier',
        field: 'bundleIdentifier',
        width: 200
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.bundleName',
        field: 'bundleName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareIdentifier',
        field: 'softwareIdentifier',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareName',
        field: 'softwareName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareVersion',
        field: 'softwareVersion',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.hostName',
        field: 'hostName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.hostUser',
        field: 'hostUser',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.localIpAddress',
        field: 'localIpAddress',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.language',
        field: 'language',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.osInfo',
        field: 'osInfo',
        width: 200
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.browserInfo',
        field: 'browserInfo',
        width: 180
      }),
      new VsGridNumberColumn({
        headerName: 'userBehaviour.durationInMinutes',
        field: 'accessDurationInMinutes',
        disableFilter: true,
        sorting: {
          disable: true
        },
        width: 250
      })
    ];
    this.grid.sizeColumnsToFit = false;
    this.grid.get = (input: VsGridGetInput) => this.get(input);
  }

  private get(input: GetAllInput): Observable<VsGridGetResult> {
    this.hasGridStarted = true;
    if (input.advancedFilter && input.advancedFilter !== '') {
      this.usageSearchAppGadgetService.updateAdvancedFilter(input.advancedFilter);
      this.usageSearchDomainGadgetService.updateAdvancedFilter(input.advancedFilter);
      this.usageSearchTenantGadgetService.updateAdvancedFilter(input.advancedFilter);
    } else {
      this.usageSearchAppGadgetService.clearAdvancedFilter();
      this.usageSearchDomainGadgetService.clearAdvancedFilter();
      this.usageSearchTenantGadgetService.clearAdvancedFilter();
    }
    return this.userBehaviourHistoricService.getAll(
      input
    ).pipe(
      map((list: LicenseUserBehaviourHistoricOutputPagedResultDto) => {
        return new VsGridGetResult(list.items, list.totalCount);
      })
    );
  }

  private subscribeToHistoryIndexed(): void {
    this.subs.add("updateGridAfterIndexed", this.userBehaviourIndexService.historicIndexSubject
      .pipe(
        filter((index) => !index?.isIndexing),
        filter(() => this.hasGridStarted)
      )
      .subscribe(() => {
        this.grid.refresh();
      }));
  }
}
