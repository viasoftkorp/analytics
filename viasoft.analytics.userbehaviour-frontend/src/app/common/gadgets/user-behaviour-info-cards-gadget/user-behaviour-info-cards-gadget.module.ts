import { NgModule } from '@angular/core';
import { VsCommonModule } from '@viasoft/common';
import { VsLabelModule, VsButtonModule } from '@viasoft/components';
import { VsGadgetModule } from '@viasoft/dashboard';
import { pt } from '../../../pages/user-behaviour/i18n/pt';
import { en } from '../../../pages/user-behaviour/i18n/en';
import { UserBehaviourInfoCardsGadgetComponent } from './user-behaviour-info-cards-gadget.component';



@NgModule({
  declarations: [UserBehaviourInfoCardsGadgetComponent],
  imports: [
    VsCommonModule.forChild({
      translates: {
        pt, en
      }
    }),
    VsLabelModule,
    VsButtonModule,
    VsGadgetModule
  ],
  exports: [UserBehaviourInfoCardsGadgetComponent],
})
export class UserBehaviourInfoCardsGadgetModule { }
