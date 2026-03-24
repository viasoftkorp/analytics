import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { VsAppCoreModule } from '@viasoft/app-core';
import { VS_BACKEND_URL } from '@viasoft/client-core';
import { MustBeLoggedAuthGuard } from '@viasoft/common';
import { API_GATEWAY, VS_UI_ERROR_SETTINGS, VsHttpModule, VsUiErrorSettings } from '@viasoft/http';
import { VsNavbarService, VsNavigationViewComponent, VsNavigationViewModule } from '@viasoft/navigation';
import { environment } from 'src/environments/environment';

import { ApiModule as DASHBOARD_API_MODULE, BASE_PATH as DASHBOARD_PATH } from '../clients/dash-board';
import { ApiModule, BASE_PATH as ANALYTICS_PATH } from '../clients/user-behaviour';
import {
  ApiModule as USER_BEHAVIOUR_ANALYTICS_API_MODULE,
  BASE_PATH as USER_BEHAVIOUR_ANALYTICS_PATH,
} from '../clients/user-behaviour-analytics';
import { en } from './i18n/en';
import { pt } from './i18n/pt';
import { NAVIGATION_ITEMS } from './navigation-items.const';
import { UserBehaviourIndexService } from './pages/user-behaviour/user-behaviour-index.service';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    VsHttpModule.forRoot({
      environment: environment,
      isCompanyBased: false
    }),
    VsAppCoreModule.forRoot({
      portalConfig: {
        appId: 'ANL01',
        appName: 'user-behaviour',
        domain: 'Customized',
        navbarTitle: 'User Behaviour'
      },
      translates: {
        en, pt
      },
      apiPrefix: 'Analytics/UserBehaviour',
      environment
    }),
    ApiModule,
    DASHBOARD_API_MODULE,
    USER_BEHAVIOUR_ANALYTICS_API_MODULE,
    RouterModule.forRoot(
      [
        {
          path: '',
          loadChildren: () => import('./pages/pages.module').then(m => m.PagesModule),
          canActivate: [MustBeLoggedAuthGuard]
        }
      ],
    ),
    VsNavigationViewModule
  ],
  providers: [
    { provide: API_GATEWAY, useFactory: () => environment['settings']['backendUrl'] },
    { provide: VS_BACKEND_URL, useFactory: () => environment['settings']['backendUrl'] },
    { provide: ANALYTICS_PATH, useFactory: () => environment['settings']['backendUrl'] },
    { provide: DASHBOARD_PATH, useFactory: () => environment['settings']['backendUrl'] },
    { provide: USER_BEHAVIOUR_ANALYTICS_PATH, useFactory: () => environment['settings']['backendUrl'] },
    { provide: VS_UI_ERROR_SETTINGS, useValue: { maxErrorModalCount: 1 } as VsUiErrorSettings },
    UserBehaviourIndexService
  ],
  bootstrap: [VsNavigationViewComponent]
})
export class AppModule {
  constructor(navigation: VsNavbarService) {
    navigation.setNavigationMenu(NAVIGATION_ITEMS);
  }
}
