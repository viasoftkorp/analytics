import { NgModule, ModuleWithProviders } from '@angular/core';
import { UserBehaviourInfoCardsGadgetModule } from '../user-behaviour-info-cards-gadget/user-behaviour-info-cards-gadget.module';
import { VsCommonModule } from '@viasoft/common';
import { VsLabelModule, VsButtonModule } from '@viasoft/components';
import { VsGadgetModule } from '@viasoft/dashboard';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { en } from '../../../pages/user-behaviour/i18n/en';
import { UserBehaviourOnlineAppsGadgetComponent } from './user-behaviour-online-apps-gadget.component';
import { UserBehaviourOnlineAppsGadgetService } from './user-behaviour-online-apps-gadget.service';



@NgModule({
  declarations: [
    UserBehaviourOnlineAppsGadgetComponent
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
    UserBehaviourOnlineAppsGadgetComponent
  ],
})
export class UserBehaviourOnlineAppsGadgetModule {
  static forChild(): ModuleWithProviders<UserBehaviourOnlineAppsGadgetModule> {
    return {
      ngModule: UserBehaviourOnlineAppsGadgetModule,
      providers: [
        UserBehaviourOnlineAppsGadgetService
      ]
    };
  }
}
