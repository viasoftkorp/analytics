import { DataLivelyUpdate } from '@viasoft/common';

export class LicenseUsageHistoryIndexUpdated implements DataLivelyUpdate {
  public readonly uniqueTypeName = 'LicenseUsageHistoryIndexUpdated';
  public isIndexing: boolean;
  public hasEsFailedSinceLastReindex: boolean;
  public lastModificationTime?: Date;

  constructor(data?: Partial<LicenseUsageHistoryIndexUpdated>) {
    this.isIndexing = data?.isIndexing;
    this.hasEsFailedSinceLastReindex = data?.hasEsFailedSinceLastReindex;
    this.lastModificationTime = data?.lastModificationTime;
  }
}
