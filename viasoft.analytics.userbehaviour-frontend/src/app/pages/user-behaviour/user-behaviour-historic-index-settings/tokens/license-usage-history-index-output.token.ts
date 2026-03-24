export class LicenseUsageHistoryIndexOutput {
  public isIndexing: boolean;
  public hasEsFailedSinceLastReindex: boolean;
  public lastModificationTime: Date;

  constructor(data?: Partial<LicenseUsageHistoryIndexOutput>) {
    this.isIndexing = data?.isIndexing;
    this.hasEsFailedSinceLastReindex = data?.hasEsFailedSinceLastReindex;
    this.lastModificationTime = data?.lastModificationTime;
  }
}
