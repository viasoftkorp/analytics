import { NgModule } from '@angular/core';
import { VsLabelModule, VsButtonModule } from '@viasoft/components';
import { VsGadgetModule } from '@viasoft/dashboard';
import { UserBehaviourInfoCardsGadgetModule } from '../user-behaviour-info-cards-gadget/user-behaviour-info-cards-gadget.module';
import { VsCommonModule } from '@viasoft/common';
import { UserBehaviourOnlineUsersGadgetComponent } from './user-behaviour-online-users-gadget.component';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { en } from '../../../pages/user-behaviour/i18n/en';


@NgModule({
  declarations: [
    UserBehaviourOnlineUsersGadgetComponent
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
  exports: [
    UserBehaviourOnlineUsersGadgetComponent
  ]
})
export class UserBehaviourOnlineUsersGadgetModule { }
