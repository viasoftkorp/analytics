import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { VsGadgetModule } from '@viasoft/dashboard';
import { UserBehaviouLicenseUsageByIdentifierGadgetComponent } from './user-behaviour-license-usage-by-identifier.component';
import { UserBehaviourLicenseUsageByIdentifierGadgetService } from './user-behaviour-license-usage-by-identifier.service';
import { NgApexchartsModule } from "ng-apexcharts";
import { VsButtonModule } from '@viasoft/components/button';
import { VsSelectModule } from '@viasoft/components/select';
import { VsFormModule } from '@viasoft/components/form';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
@NgModule({
  declarations: [
    UserBehaviouLicenseUsageByIdentifierGadgetComponent],
  imports: [
    VsCommonModule.forChild({
      translates: {}
    }),
    VsGadgetModule,
    VsButtonModule,
    NgApexchartsModule,
    VsSelectModule,
    FormsModule,
    ReactiveFormsModule,
    VsFormModule
  ],
  providers: [UserBehaviourLicenseUsageByIdentifierGadgetService],
  exports: [UserBehaviouLicenseUsageByIdentifierGadgetComponent]
})
export class UserBehaviourLicenseUsageByIdentifierGadgetModule { }
