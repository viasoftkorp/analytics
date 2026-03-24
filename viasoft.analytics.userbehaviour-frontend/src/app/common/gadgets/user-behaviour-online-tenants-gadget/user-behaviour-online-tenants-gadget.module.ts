import { NgModule } from '@angular/core';
import { UserBehaviourOnlineTenantsGadgetComponent } from './user-behaviour-online-tenants-gadget.component';
import { VsCommonModule } from '@viasoft/common';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { en } from '../../../pages/user-behaviour/i18n/en';
import { VsButtonModule, VsLabelModule } from '@viasoft/components';
import { UserBehaviourOnlineTenantsGadgetService } from './user-behaviour-online-tenants-gadget.service';
import { VsGadgetModule } from '@viasoft/dashboard';
import { UserBehaviourInfoCardsGadgetModule } from '../user-behaviour-info-cards-gadget/user-behaviour-info-cards-gadget.module';



@NgModule({
  declarations: [
    UserBehaviourOnlineTenantsGadgetComponent
  ],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt, en
      }
    }),
    VsLabelModule,
    VsButtonModule,
    VsGadgetModule,
    UserBehaviourInfoCardsGadgetModule
  ],
  providers: [
    UserBehaviourOnlineTenantsGadgetService
  ],
  exports: [
    UserBehaviourOnlineTenantsGadgetComponent
  ]
})
export class UserBehaviourOnlineTenantsGadgetModule { }
