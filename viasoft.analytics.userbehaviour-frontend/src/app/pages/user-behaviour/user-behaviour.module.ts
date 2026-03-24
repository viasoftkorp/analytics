import { NgModule } from '@angular/core';
import { DateAdapter, MAT_DATE_FORMATS } from '@angular/material/core';
import { VsCommonModule } from '@viasoft/common';
import { VsButtonModule } from '@viasoft/components/button';
import { VsGridModule } from '@viasoft/components/grid';
import { VsLabelModule } from '@viasoft/components/label';
import { VsLayoutModule } from '@viasoft/components/layout';
import { VS_DATE_FORMAT, VsDateAdapter } from '@viasoft/components/shared';
import { VsSpinnerModule } from '@viasoft/components/spinner';
import { VsTabGroupModule } from '@viasoft/components/tab-group';
import { VsDashboardApiService, VsDashboardModule, VsDashboardService, VsGadgetModule } from '@viasoft/dashboard';
import { TabsViewTemplateModule } from '@viasoft/view-template';
import { GADGETS } from 'src/app/common/gadgets/gadgets';
import {
  UsageSearchAppGadgetComponent,
} from 'src/app/common/gadgets/usage-search-app-gadget/usage-search-app-gadget.component';
import { UsageSearchAppGadgetModule } from 'src/app/common/gadgets/usage-search-app-gadget/usage-search-app-gadget.module';
import {
  UsageSearchDomainGadgetComponent,
} from 'src/app/common/gadgets/usage-search-domain-gadget/usage-search-domain-gadget.component';
import {
  UsageSearchDomainGadgetModule,
} from 'src/app/common/gadgets/usage-search-domain-gadget/usage-search-domain-gadget.module';
import {
  UsageSearchTenantGadgetComponent,
} from 'src/app/common/gadgets/usage-search-tenant-gadget/usage-search-tenant-gadget.component';
import {
  UsageSearchTenantGadgetModule,
} from 'src/app/common/gadgets/usage-search-tenant-gadget/usage-search-tenant-gadget.module';
import {
  UserBehaviouLicenseUsageByIdentifierGadgetComponent,
} from 'src/app/common/gadgets/user-behaviour-license-usage-by-identifier/user-behaviour-license-usage-by-identifier.component';
import {
  UserBehaviourLicenseUsageByIdentifierGadgetModule,
} from 'src/app/common/gadgets/user-behaviour-license-usage-by-identifier/user-behaviour-license-usage-by-identifier.module';
import {
  UserBehaviourOnlineAppsGadgetComponent,
} from 'src/app/common/gadgets/user-behaviour-online-apps-gadget/user-behaviour-online-apps-gadget.component';
import {
  UserBehaviourOnlineAppsGadgetModule,
} from 'src/app/common/gadgets/user-behaviour-online-apps-gadget/user-behaviour-online-apps-gadget.module';
import {
  UserBehaviourOnlineTenantsGadgetComponent,
} from 'src/app/common/gadgets/user-behaviour-online-tenants-gadget/user-behaviour-online-tenants-gadget.component';
import {
  UserBehaviourOnlineTenantsGadgetModule,
} from 'src/app/common/gadgets/user-behaviour-online-tenants-gadget/user-behaviour-online-tenants-gadget.module';
import {
  UserBehaviourOnlineUsersGadgetComponent,
} from 'src/app/common/gadgets/user-behaviour-online-users-gadget/user-behaviour-online-users-gadget.component';
import {
  UserBehaviourOnlineUsersGadgetModule,
} from 'src/app/common/gadgets/user-behaviour-online-users-gadget/user-behaviour-online-users-gadget.module';
import { UserBehaviourDashboardService } from 'src/app/common/services/user-behaviour-dashboard.service';

import { en } from './i18n/en';
import { pt } from './i18n/pt';
import { UserBehaviourDashboardComponent } from './user-behaviour-dashboard/user-behaviour-dashboard.component';
import {
  UserBehaviourHistoricIndexSettingsComponent,
} from './user-behaviour-historic-index-settings/user-behaviour-historic-index-settings.component';
import { UserBehaviourHistoricComponent } from './user-behaviour-historic/user-behaviour-historic.component';
import { UserBehaviourHistoricService } from './user-behaviour-historic/user-behaviour-historic.service';
import { UserBehaviourRealTimeComponent } from './user-behaviour-real-time/user-behaviour-real-time.component';
import { UserBehaviourRealTimeService } from './user-behaviour-real-time/user-behaviour-real-time.service';
import { UserBehaviourRoutingModule } from './user-behaviour-routing.module';
import { UserBehaviourComponent } from './user-behaviour.component';
import { UserBehaviourService } from './user-behaviour.service';
import {
  UsageSearchDatabaseGadgetModule
} from '../../common/gadgets/usage-search-database-version-gadget/usage-search-database-gadget.module';
import {
  UsageSearchDatabaseGadgetComponent
} from '../../common/gadgets/usage-search-database-version-gadget/usage-search-database-gadget.component';

@NgModule({
  declarations: [
    UserBehaviourDashboardComponent,
    UserBehaviourComponent,
    UserBehaviourRealTimeComponent,
    UserBehaviourHistoricComponent,
    UserBehaviourHistoricIndexSettingsComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt, en
      }
    }),
    UserBehaviourLicenseUsageByIdentifierGadgetModule,
    UserBehaviourRoutingModule,
    TabsViewTemplateModule,
    UserBehaviourOnlineTenantsGadgetModule,
    UserBehaviourOnlineUsersGadgetModule,
    UsageSearchAppGadgetModule,
    UsageSearchDomainGadgetModule,
    UsageSearchTenantGadgetModule,
    UsageSearchDatabaseGadgetModule,
    VsButtonModule,
    VsLabelModule,
    VsSpinnerModule,
    VsTabGroupModule,
    VsGridModule,
    VsLayoutModule,
    UserBehaviourOnlineAppsGadgetModule.forChild(),
    VsDashboardModule.forChild({
      gadgetsDataSource: [
        ...GADGETS
      ]
    }),
    VsGadgetModule.config({
      gadgetComponentDefinition: {
        UserBehaviourOnlineTenantsGadget: UserBehaviourOnlineTenantsGadgetComponent,
        UserBehaviourOnlineUsersGadget: UserBehaviourOnlineUsersGadgetComponent,
        UserBehaviourOnlineAppsGadget: UserBehaviourOnlineAppsGadgetComponent,
        UserBehaviourUsageSearchAppGadget: UsageSearchAppGadgetComponent,
        UserBehaviourUsageSearchDomainGadget: UsageSearchDomainGadgetComponent,
        UserBehaviourUsageSearchTenantGadget: UsageSearchTenantGadgetComponent,
        UsageSearchDatabaseGadget: UsageSearchDatabaseGadgetComponent,
        UserBehaviouLicenseUsageByIdentifierGadget: UserBehaviouLicenseUsageByIdentifierGadgetComponent
      }
    })
  ],
  providers: [
    { provide: VsDashboardService, useClass: UserBehaviourDashboardService },
    VsDashboardApiService,
    UserBehaviourRealTimeService,
    UserBehaviourService,
    UserBehaviourHistoricService,
    {
      provide: DateAdapter,
      useClass: VsDateAdapter
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: VS_DATE_FORMAT
    }
  ],
  exports: [
    UserBehaviourDashboardComponent,
    UserBehaviourRealTimeComponent,
    UserBehaviourHistoricComponent,
    UserBehaviourHistoricIndexSettingsComponent
  ]
})

export class UserBehaviourModule { }
