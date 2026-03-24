import { Directive, Injector, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { GadgetBase } from '@viasoft/dashboard';
import { ChartComponent } from 'ng-apexcharts';
import { Subscription } from 'rxjs';
import { FilterByGroupingKey, FilterByGroupingOutput } from 'src/clients/user-behaviour-analytics';
import { UsageSearchBaseGadgetService } from './usage-search-base-gadget.service';
import { UsageSearchFilter } from './usage-search-filter';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { VsSelectOption } from '@viasoft/components/select';
import { VsDialog } from '@viasoft/components';
import { ApexOptions } from 'ng-apexcharts/lib/model/apex-types';

@Directive()
export abstract class UsageSearchBaseGadget extends GadgetBase implements OnInit, OnDestroy {
  public subs = new  VsSubscriptionManager();
  @ViewChild('chartObj') chartObj: ChartComponent;
  public chartOptions: Partial<ApexOptions>;
  currentLanguage: any;
  isChartReady = false;
  data: number[];
  completeData: FilterByGroupingKey[] = [];
  public form: FormGroup;
  public dateTimeIntervalOptions: VsSelectOption[] = [
    {
      value: 1,
      name: 'Hoje'
    } as VsSelectOption,
    {
      value: 2,
      name: 'Últimos 7 Dias'
    } as VsSelectOption,
    {
      value: 3,
      name: 'Últimos 30 Dias'
    } as VsSelectOption,
    {
      value: 4,
      name: 'Últimos 90 Dias'
    } as VsSelectOption,
    {
      value: 5,
      name: 'Últimos 6 Meses'
    } as VsSelectOption,
    {
      value: 6,
      name: 'Últimos 12 Meses'
    } as VsSelectOption,
  ];
  public appliedDateInterval: number;

  constructor(injector: Injector,
              private readonly translationService: TranslateService,
              private readonly usageSearchGadgetService: UsageSearchBaseGadgetService,
              private readonly formBuilder: FormBuilder,
              private dialog: VsDialog
  ) {
    super(injector);
    const filter = this.usageSearchGadgetService.getFilterFromCache();
    this.form = this.formBuilder.group({
      dateInterval: [filter?.dateInterval, []],
      maxResultCount: [filter?.maxResultCount, []]
    });
    this.appliedDateInterval = filter.dateInterval;
    this.subs.add("search", this.createChart());
  }

  ngOnDestroy(): void {
    this.subs.clear();
  }

  public applyFilter() {
    const filter = this.form.getRawValue() as UsageSearchFilter;
    this.usageSearchGadgetService.setFilterToCache(filter);
    this.updateChart();
    this.appliedDateInterval = filter.dateInterval;
    this.dialog.close();
  }
  updateChart() {
    const filter = this.usageSearchGadgetService.getFilterFromCache();
    if (this.subs.subscriptions.search && !this.subs.subscriptions.search.closed) {
      this.subs.clear();
    }
    this.subs.add("search", this.usageSearchGadgetService .filterByGrouping(filter).subscribe(
      (r) => {
        this.data = r.values.map(v => v.value);
        this.completeData = [...r.values];
        const categories = this.getCategories(r);
        this.updateChartData(categories);
      }));
  }

  protected createChart(): Subscription {
    const filter = this.usageSearchGadgetService.getFilterFromCache();
    return this.usageSearchGadgetService.filterByGrouping(filter).subscribe(
      (r) => {
        this.data = r.values.map(v => v.value);
        this.completeData = [...r.values];
        const categories = this.getCategories(r);

        this.chartOptions = {
          series: [
            {
              name: this.translationService.instant('dashBoard.usageSearch.access'),
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
            categories
          }
        };
        this.isChartReady = true;
      }
    );
  }

  updateChartData(categories: Array<string>) {
    if (this.data.length !== 0) {
      this.chartOptions.series = [
        {
          name: this.translationService.instant('dashBoard.usageSearch.access'),
          data: this.data
        }
      ];
      this.chartOptions.xaxis = {
        categories
      };
      this.chartObj.updateSeries([
        {
          name: this.translationService.instant('dashBoard.usageSearch.access'),
          data: this.data
        }
      ]);
    } else {
      const array = new Array(this.chartOptions.xaxis.categories.length).fill(0);
      if (this.chartObj !== undefined) {
        this.chartObj.updateSeries([
          {
            name: this.translationService.instant('dashBoard.usageSearch.access'),
            data: array
          }
        ]);
      }
    }
  }

  abstract getCategories(input: FilterByGroupingOutput);

  abstract get currentGroupingLabel();

  numberToPercentage(val: number): string {
    let sum = 0;
    this.data.forEach(d => {
      sum = sum + d;
    });
    const percentage = val / sum * 100;
    return percentage.toFixed(2) + '%';
  }

  run(): void {
    this.initializeRunState(true);
    this.updateData(null);
  }
  stop(): void {
    this.setStopState(false);
  }
  updateProperties(updatedProperties: any): void {}

  updateData(data: any[]): void {}

  preRun(): void {
    this.run();
  }
}
