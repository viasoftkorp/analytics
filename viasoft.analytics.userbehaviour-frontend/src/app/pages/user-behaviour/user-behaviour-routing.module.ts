import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { UserBehaviourDashboardComponent } from './user-behaviour-dashboard/user-behaviour-dashboard.component';
import {
  UserBehaviourHistoricIndexSettingsComponent,
} from './user-behaviour-historic-index-settings/user-behaviour-historic-index-settings.component';
import { UserBehaviourHistoricComponent } from './user-behaviour-historic/user-behaviour-historic.component';
import { UserBehaviourRealTimeComponent } from './user-behaviour-real-time/user-behaviour-real-time.component';
import { UserBehaviourComponent } from './user-behaviour.component';

const routes: Routes = [
  {
    path: '',
    component: UserBehaviourComponent,
    children: [
      {
        path: 'dashboard',
        component: UserBehaviourDashboardComponent
      },
      {
        path: 'historic',
        component: UserBehaviourHistoricComponent
      },
      {
        path: 'historic-index-settings',
        component: UserBehaviourHistoricIndexSettingsComponent
      },
      {
        path: '**',
        component: UserBehaviourRealTimeComponent
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(
      routes
    )
  ]
})
export class UserBehaviourRoutingModule { }
