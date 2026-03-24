import { VsDashboardService, VsDashboardDataSource, VsBoardDataSource, VsGadgetDataSource, VsDashboardApiService, VsDashboardInput } from '@viasoft/dashboard';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IGadget } from '@viasoft/dashboard/lib/gadgets/common/igadget';
import { GADGETS } from '../gadgets/gadgets';
import { DashboardInput } from 'src/clients/dash-board';
import { map } from 'rxjs/operators';

@Injectable()
export class UserBehaviourDashboardService extends VsDashboardService {

    dashboardInput: DashboardInput = {
      consumerId: '2CE42662-528A-4083-957B-B6D92F1980BF'
    };

    constructor(private dashBoardServiceProxy: VsDashboardApiService) {
      super(GADGETS);
    }

    getGadgets(): Observable<VsGadgetDataSource[]> {
      return of(GADGETS);
    }

    getBoardLayouts(): Observable<VsBoardDataSource[]> {
      return of([
          {
            id: 100,
            title: 'One Column',
            structure: '100',
            rows: [{
              columns: [{
                flex: '100',
              }]
            }]
          } as VsBoardDataSource,
          {
            id: 400,
            title: 'Two Columns',
            structure: '100/50-50',
            rows: [{
              columns: [{
                flex: '100',
                gadgets: []
              }]
            }, {
              columns: [{
                flex: '50',
                gadgets: []
              }, {
                flex: '50',
                gadgets: []
              }]
            }]
          } as VsBoardDataSource,
          {
            id: 450,
            title: 'Tree Columns',
            structure: '50-50/50-50',
            rows: [{
              columns: [{
                flex: '50',
                gadgets: []
              }, {
                flex: '50',
                gadgets: []
              }]
            }, {
              columns: [{
                flex: '50',
                gadgets: []
              }, {
                flex: '50',
                gadgets: []
              }]
            }]
          } as VsBoardDataSource,
          {
            id: 1188,
            title: 'Tree Columns',
            structure: '33-33-33/33-33-33/33-33-33',
            rows: [{
              columns: [{
                flex: '33.333333333333336',
                gadgets: [],
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }]
            }, {
              columns: [{
                flex: '33.333333333333336',
                gadgets: []
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }]
            }, {
              columns: [{
                flex: '33.333333333333336',
                gadgets: []
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }, {
                flex: '33.333333333333336',
                gadgets: []
              }]
            }]
          } as VsBoardDataSource,
        ]);
    }

    getDashboard(): Observable<VsDashboardDataSource> {
      return this.dashBoardServiceProxy.getDashboard(this.dashboardInput.consumerId)
      .pipe(map(dashboardSource => {
        if (dashboardSource) {
          return VsDashboardDataSource.fromJS(JSON.parse(dashboardSource.dashboardDataSource));
        }
      }));
    }

    updateDashboard(dataToUpdate: VsDashboardDataSource): Observable<boolean> {
      const update = JSON.stringify(dataToUpdate);
      const input = <VsDashboardInput>{
        dashboardDataSource: update,
        consumerId: this.dashboardInput.consumerId
      };
      return this.dashBoardServiceProxy.updateDashboard(input);
    }

    restoreDashboard(): Observable<void> {
      return this.dashBoardServiceProxy.restoreDashboard('2CE42662-528A-4083-957B-B6D92F1980BF');
    }

    setAsDefaultBoardDashboard(board: VsDashboardDataSource): Observable<void> {
      const update = JSON.stringify(board);
      const input = <VsDashboardInput>{
        dashboardDataSource: update,
        consumerId: this.dashboardInput.consumerId
      };
      return this.dashBoardServiceProxy.setAsDefaultBoardDashboard(input);
    }

    getGadgetComponentByClassName(gadgetName: string): Observable<IGadget> {
      return of();
    }
}
