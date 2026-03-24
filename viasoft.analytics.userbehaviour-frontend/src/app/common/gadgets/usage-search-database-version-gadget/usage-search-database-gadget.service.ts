import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_GATEWAY, ensureTrailingSlash } from '@viasoft/http';
import { Observable } from 'rxjs';
import { DatabaseVersionCount } from './tokens/database-version.count';

@Injectable({
  providedIn: 'root'
})
export class UsageSearchDatabaseGadgetService {
  constructor(private httpClient: HttpClient, @Inject(API_GATEWAY) private gateway: string) { }

  public get basePath(): string {
    return `${ensureTrailingSlash(this.gateway)}analytics/userbehaviour`;
  }

  public getdatabaseVersionCountByAccount(): Observable<DatabaseVersionCount[]> {
    return this.httpClient.get<DatabaseVersionCount[]>(`${this.basePath}/database-version-count`,
      {});
  }
}

