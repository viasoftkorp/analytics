import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { VsFormModule } from '@viasoft/components/form';
import { VsIconModule } from '@viasoft/components/icon';
import { VsLabelModule } from '@viasoft/components/label';
import { VsSelectModule } from '@viasoft/components/select';
import { VsSpinnerModule } from '@viasoft/components/spinner';
import { VsGadgetModule } from '@viasoft/dashboard';
import { NgApexchartsModule } from 'ng-apexcharts';

import { VsButtonModule } from '@viasoft/components';
import { en } from '../../../pages/user-behaviour/i18n/en';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { UsageSearchDatabaseGadgetComponent } from './usage-search-database-gadget.component';
import { UsageSearchDatabaseGadgetService } from './usage-search-database-gadget.service';

@NgModule({
  declarations: [UsageSearchDatabaseGadgetComponent],
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
  ],
  exports: [
    UsageSearchDatabaseGadgetComponent
  ],
  providers: [
    UsageSearchDatabaseGadgetService
  ]
})
export class UsageSearchDatabaseGadgetModule { }
