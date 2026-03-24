import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import {
  VsButtonModule,
  VsFormModule,
  VsIconModule, VsInputModule,
  VsLabelModule,
  VsSelectModule,
  VsSpinnerModule,
} from '@viasoft/components';
import { VsGadgetModule } from '@viasoft/dashboard';
import { NgApexchartsModule } from 'ng-apexcharts';

import { en } from '../../../pages/user-behaviour/i18n/en';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { UsageSearchAppGadgetComponent } from './usage-search-app-gadget.component';
import { UsageSearchAppGadgetService } from './usage-search-app-gadget.service';

@NgModule({
  declarations: [UsageSearchAppGadgetComponent],
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
    VsLabelModule,
    VsSpinnerModule,
    VsIconModule,
    VsFormModule,
    VsButtonModule,
    VsInputModule,
  ],
  exports: [
    UsageSearchAppGadgetComponent
  ],
  providers: [
    UsageSearchAppGadgetService
  ]
})
export class UsageSearchAppGadgetModule { }
