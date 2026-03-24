import { Injectable } from '@angular/core';
import { LicenseUserBehaviourServiceProxy } from 'src/clients/user-behaviour/api/licenseUserBehaviourServiceProxy';
import { Observable } from 'rxjs';
import { LicenseUserBehaviourOutputPagedResultDto } from 'src/clients/user-behaviour/model/licenseUserBehaviourOutputPagedResultDto';
import { GetAllInput } from '../../../common/inputs/getAllInput';

@Injectable()
export class UserBehaviourRealTimeService {

  constructor(private licenseUserBehaviourServiceproxy: LicenseUserBehaviourServiceProxy) { }

  getUsersBehaviour(input: GetAllInput): Observable<LicenseUserBehaviourOutputPagedResultDto> {
    return this.licenseUserBehaviourServiceproxy.getUsersBehaviour(
      'America/Sao_Paulo',
      null,
      input.filter,
      input.advancedFilter,
      input.sorting,
      input.skipCount,
      input.maxResultCount
    );
  }

}
