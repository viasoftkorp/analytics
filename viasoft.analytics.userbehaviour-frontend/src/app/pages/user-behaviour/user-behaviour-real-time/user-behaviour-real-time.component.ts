import { Component, OnInit } from '@angular/core';
import {
  VsGridDateTimeColumn,
  VsGridGetInput,
  VsGridGetResult,
  VsGridOptions,
  VsGridSimpleColumn,
} from '@viasoft/components/grid';
import { map } from 'rxjs/operators';
import { Domain } from 'src/app/common/enums/domains.enum';
import {
  LicenseUserBehaviourOutputPagedResultDto,
} from 'src/clients/user-behaviour/model/licenseUserBehaviourOutputPagedResultDto';

import { GetAllInput } from '../../../common/inputs/getAllInput';
import { UserBehaviourService } from '../user-behaviour.service';
import { UserBehaviourRealTimeService } from './user-behaviour-real-time.service';


@Component({
  selector: 'app-user-behaviour-real-time',
  templateUrl: './user-behaviour-real-time.component.html',
  styleUrls: ['./user-behaviour-real-time.component.scss']
})
export class UserBehaviourRealTimeComponent implements OnInit {
  grid: VsGridOptions;
  curentTime: Date;
  gadgetHasOperationControls = false;
  isLoading = true;
  constructor(private readonly userBehaviourRealTimeService: UserBehaviourRealTimeService,
              private readonly userBehaviourService: UserBehaviourService) {
  }

  ngOnInit(): void {
    this.configureGrid();

    setInterval(() => {
      this.grid.refresh();
    }, 60000);
  }

  private configureGrid(): void {
    this.grid = new VsGridOptions();
    this.grid.id = '9E34DC5C-4E3B-455C-B8B2-D35F11AB8603';
    this.grid.sizeColumnsToFit = false;

    this.grid.columns = [
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.licensingIdentifier',
        field: 'licensingIdentifier',
        disableFilter: true,
        width: 260
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.accountName',
        field: 'accountName'
      }),
      new VsGridDateTimeColumn({
        headerName: 'userBehaviour.lastUpdate',
        field: 'lastUpdate',
        disableFilter: true,
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.domain.domain',
        field: 'domain',
        translate: true,
        width: 150,
        format: (value) => {
          return 'userBehaviour.domain.domains.' + Domain[value];
        }
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.databaseName',
        field: 'databaseName',
        width: 260
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.user',
        field: 'user',
        width: 150
      }),
      new VsGridDateTimeColumn({
        headerName: 'userBehaviour.startTime',
        field: 'startTime',
        disableFilter: true,
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.accessDuration',
        field: 'accessDurationFormatted',
        disableFilter: true,
        sorting: {
          disable: true
        },
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.appIdentifier',
        field: 'appIdentifier',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.appName',
        field: 'appName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.bundleIdentifier',
        field: 'bundleIdentifier',
        width: 200
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.bundleName',
        field: 'bundleName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareIdentifier',
        field: 'softwareIdentifier',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareName',
        field: 'softwareName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.softwareVersion',
        field: 'softwareVersion',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.hostName',
        field: 'hostName',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.hostUser',
        field: 'hostUser',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.localIpAddress',
        field: 'localIpAddress',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.language',
        field: 'language',
        width: 180
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.osInfo',
        field: 'osInfo',
        width: 200
      }),
      new VsGridSimpleColumn({
        headerName: 'userBehaviour.browserInfo',
        field: 'browserInfo',
        width: 180
      })
    ];
    this.grid.get = (input: VsGridGetInput) => this.get(input);
  }

  private get(input: GetAllInput) {
    if (input.advancedFilter && input.advancedFilter !== '') {
      this.userBehaviourService.notifyAddSubject(input);
    } else {
      this.userBehaviourService.notifyClearSubject();
    }
    return this.userBehaviourRealTimeService.getUsersBehaviour(
      input
    ).pipe(
      map(
        (list: LicenseUserBehaviourOutputPagedResultDto) => new VsGridGetResult(list.items, list.totalCount)
      )
    );
  }
}
