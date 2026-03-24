import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { VsFormModule } from '@viasoft/components/form';
import { VsIconModule } from '@viasoft/components/icon';
import { VsLabelModule } from '@viasoft/components/label';
import { VsSelectModule } from '@viasoft/components/select';
import { VsSpinnerModule } from '@viasoft/components/spinner';
import { VsGadgetModule } from '@viasoft/dashboard';
import { NgApexchartsModule } from 'ng-apexcharts';

import { en } from '../../../pages/user-behaviour/i18n/en';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { UsageSearchTenantGadgetComponent } from './usage-search-tenant-gadget.component';
import { UsageSearchTenantGadgetService } from './usage-search-tenant-gadget.service';
import { VsButtonModule, VsInputModule } from '@viasoft/components';

@NgModule({
  declarations: [UsageSearchTenantGadgetComponent],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt,
        en,
      },
    }),
    NgApexchartsModule,
    VsGadgetModule,
    VsSelectModule,
    VsSpinnerModule,
    VsLabelModule,
    VsIconModule,
    VsFormModule,
    VsButtonModule,
    VsInputModule,
  ],
  exports: [
    UsageSearchTenantGadgetComponent
  ],
  providers: [
    UsageSearchTenantGadgetService
  ]
})
export class UsageSearchTenantGadgetModule { }
