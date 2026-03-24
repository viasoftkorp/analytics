import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GetAllInput } from 'src/app/common/inputs/getAllInput';
import {
  LicenseUsageHistoryOutputPagedResultDto,
  LicenseUsageHistoryServiceProxy,
} from 'src/clients/user-behaviour-analytics';

@Injectable()
export class UserBehaviourHistoricService {

  constructor(private readonly licenseUsageHistoryServiceProxy: LicenseUsageHistoryServiceProxy) { }

  getAll(input: GetAllInput): Observable<LicenseUsageHistoryOutputPagedResultDto> {
    return this.licenseUsageHistoryServiceProxy.getAll(
      input.filter,
      input.advancedFilter,
      input.sorting,
      input.skipCount,
      input.maxResultCount
    );
  }

}
