import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Subscription } from 'rxjs';

import { UsageSearchDatabaseGadgetService } from './usage-search-database-gadget.service';
import { ApexOptions } from 'ng-apexcharts/lib/model/apex-types';
import { GadgetBase } from '@viasoft/dashboard';

@Component({
  selector: 'vs-dynamic-component',
  templateUrl: './usage-search-database-gadget.component.html',
  styleUrls: ['./usage-search-database-gadget.component.scss']
})
export class UsageSearchDatabaseGadgetComponent extends GadgetBase implements OnInit, OnDestroy {
  run(): void {
    this.initializeRunState(true);
    this.updateData(null);
    this.createChart();
  }
  stop(): void {
    this.setStopState(false);
  }
  updateProperties(updatedProperties: any): void {}

  updateData(data: any[]): void {}

  preRun(): void {
    this.run();
  }

  public isChartReady = false;
  public data: number[];
  public chartOptions: Partial<ApexOptions>;

  constructor(
    injector: Injector,
    private translateService: TranslateService,
    private usageSearchDatabaseGadgetService: UsageSearchDatabaseGadgetService,
  ) {
    super(injector);
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  protected createChart(): Subscription {
    return this.usageSearchDatabaseGadgetService.getdatabaseVersionCountByAccount().subscribe(
      (r) => {
        this.data = r.map(v => v.count);
        this.chartOptions = {
          series: [
            {
              name: this.translateService.instant('dashBoard.usageSearch.accounts'),
              data: this.data
            }
          ],
          chart: {
            type: 'bar',
            height: 350,
            toolbar: {
              tools: {
                download: false
              }
            }
          },
          plotOptions: {
            bar: {
              horizontal: true,
              dataLabels: {
                position: 'top'
              }
            }
          },
          dataLabels: {
            enabled: true,
            style: {
              colors: ['#333']
            },
            offsetX: 50,
            textAnchor: 'start',
            formatter: (val) => {
              return this.numberToPercentage(Number(val));
            }
          },
          xaxis: {
            categories: [...r.map(v => v.databaseVersion)]
          }
        };
        this.isChartReady = true;
      }
    );
  }
  numberToPercentage(val: number): string {
    let sum = 0;
    this.data.forEach(d => {
      sum = sum + d;
    });
    const percentage = val / sum * 100;
    return percentage.toFixed(2) + '%';
  }
}
