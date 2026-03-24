import { Component, Injector, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { VsSubscriptionManager } from '@viasoft/common';
import { VsSelectOption } from '@viasoft/components/select';
import { GadgetBase } from '@viasoft/dashboard';
import { ChartComponent } from 'ng-apexcharts';
import { LicensesInUsageCountOutput } from 'src/clients/user-behaviour';

import { getNormalizedHourMinute, getNormalizedInterval, getNormalizedTime } from '../../functions/normalize-times';
import { ChartOptions } from '../../tokens/apex-chart';
import { constructGraph, graphTooltip } from './chart-options';
import { UserBehaviourLicenseUsageByIdentifierGadgetService } from './user-behaviour-license-usage-by-identifier.service';

const defaultSliceTime = 4;
const minorMinuteInterval = 5;

@Component({
  selector: 'app-user-behaviour-license-usage-by-identifier',
  templateUrl: './user-behaviour-license-usage-by-identifier.component.html',
  styleUrls: ['./user-behaviour-license-usage-by-identifier.component.scss']
})
export class UserBehaviouLicenseUsageByIdentifierGadgetComponent extends GadgetBase implements OnInit, OnDestroy {

  // Graph Options
  @ViewChild("chart") chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;
  defaultGraphInterval = minorMinuteInterval;
  graphData: {
    name: string;
    data: { x: string; y: number; id: string; }[]
  } = { name: this.translateService.instant('dashBoard.licenseUsage.title'), data: [] };
  finishGettingGraphData = false;

  // Form and component validations
  form: FormGroup;
  leftShiftIteractions = [];
  selection: VsSelectOption[] = [{ name: 'dashBoard.licenseUsage.interval.fiveMinutes', value: 5 }, { name: 'dashBoard.licenseUsage.interval.tenMinutes', value: 10 },
  { name: 'dashBoard.licenseUsage.interval.fifteenMinutes', value: 15 }, { name: 'dashBoard.licenseUsage.interval.thirtyMinutes', value: 30 },
  { name: 'dashBoard.licenseUsage.interval.sixtyMinutes', value: 60 }];
  localStorageFilters: {
    interval: number,
    advancedFilter: string
  };
  private subs = new VsSubscriptionManager();
  private timeInterval;
  public disableLeftNavigation = false;

  constructor(injector: Injector, private userBehaviourLicenseUsageByIdentifierGadgetService: UserBehaviourLicenseUsageByIdentifierGadgetService,
    private fb: FormBuilder, private translateService: TranslateService) {
    super(injector);
    this.initiateStorage();
    this.initiateForm();
  }

  ngOnInit(): void {
    this.subs['request_usage'] = this.userBehaviourLicenseUsageByIdentifierGadgetService.getUsageCount(this.localStorageFilters.advancedFilter).subscribe((res: LicensesInUsageCountOutput[]) => {
      this.buildGraph(res);
    });
    this.subs['updated_advanced_filter'] = this.userBehaviourLicenseUsageByIdentifierGadgetService.advancedFilterChangedSubject.subscribe(() => {
      this.initiateStorage();
    });
    this.graphIntervalRefresh();
  }

  ngOnDestroy() {
    this.subs.clear();
    clearInterval(this.timeInterval);
  }

  private initiateStorage() {
    this.localStorageFilters = this.userBehaviourLicenseUsageByIdentifierGadgetService.getLocalStorage();
    if (this.localStorageFilters) {
      this.defaultGraphInterval = this.localStorageFilters.interval;
    } else {
      this.localStorageFilters = {
        interval: this.defaultGraphInterval,
        advancedFilter: null
      };
    }
  }

  private initiateForm() {
    this.form = this.fb.group({
      select: [this.defaultGraphInterval, [Validators.required]]
    });
  }

  private graphIntervalRefresh() {
    if (this.timeInterval) {
      clearInterval(this.timeInterval);
    }
    const refreshInterval = (this.defaultGraphInterval + 1) * 60 * 100;
    this.timeInterval = setInterval(() => {
      this.refreshGraphData(this.localStorageFilters.advancedFilter);
    }, refreshInterval);
  }

  private buildGraph(data: LicensesInUsageCountOutput[]) {
    this.insertDayPeriods();
    data.forEach(element => {
      const startTime = new Date(element.startInterval);
      const endTime = new Date(element.endInterval);
      this.insertInterval(startTime, endTime, element.usageCount);
    });
    this.constructGraph();
    this.finishGettingGraphData = true;
  }

  private insertDayPeriods() {
    this.graphData = { name: this.translateService.instant('dashBoard.licenseUsage.title'), data: [] };
    for (let horaInicial = 0; horaInicial < 24; horaInicial++) {
      let lastFillMinute = 0;
      for (let minutesInterval = 0; minutesInterval < (60 / this.defaultGraphInterval); minutesInterval++) {
        const normalizedHour = getNormalizedTime(horaInicial);
        const normalizedMinute = getNormalizedTime(lastFillMinute);
        this.graphData.data.push({ x: getNormalizedHourMinute(normalizedHour, normalizedMinute), y: 0, id: getNormalizedHourMinute(normalizedHour, normalizedMinute) });

        const emptyValues = (this.defaultGraphInterval / minorMinuteInterval) - 1;
        let lastEmptyMinuteValue = lastFillMinute + minorMinuteInterval;
        for (let index = 0; index < emptyValues; index++) {
          const normalizedEmptyMinute = getNormalizedTime(lastEmptyMinuteValue);
          this.graphData.data.push({ x: "", y: 0, id: getNormalizedHourMinute(normalizedHour, normalizedEmptyMinute) });
          lastEmptyMinuteValue += minorMinuteInterval;
        }
        lastFillMinute = this.defaultGraphInterval + lastFillMinute;
      }
    }
  }

  private insertInterval(startTime: Date, endTime: Date, usageCount: number) {
    const normalizedDateInterval = getNormalizedInterval(startTime, endTime);
    const stringStarTime = normalizedDateInterval.startTimeString;
    const stringEndTime = normalizedDateInterval.endTimeString;
    const graphIndexInCurrentStartInterval = this.graphData.data.find(data => data.id == stringStarTime);
    const graphIndexInCurrentEndInterval = this.graphData.data.find(data => data.id == stringEndTime);

    if (graphIndexInCurrentStartInterval && graphIndexInCurrentStartInterval.y != usageCount) {
      graphIndexInCurrentStartInterval.y = usageCount;
    }

    if (graphIndexInCurrentEndInterval && graphIndexInCurrentEndInterval.y != usageCount) {
      graphIndexInCurrentEndInterval.y = usageCount;
    }
  }

  leftShift() {
    let finalHour;
    if (this.leftShiftIteractions.length === 0) {
      finalHour = new Date().getHours() - 1;
    } else {
      finalHour = this.leftShiftIteractions[this.leftShiftIteractions.length - 1] - 1;
    }
    this.leftShiftIteractions.push(finalHour);
    const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour(finalHour).data;
    this.updateGraphSeriesAndOptions(sliceOfGraphDataByCurrentHour);
  }

  rightShift() {
    let finalHour = this.leftShiftIteractions[this.leftShiftIteractions.length - 1] + 1;
    const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour(finalHour).data;
    this.leftShiftIteractions.pop();
    this.updateGraphSeriesAndOptions(sliceOfGraphDataByCurrentHour);
  }

  goStart() {
    let currentHour = new Date().getHours();
    const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour(defaultSliceTime).data;
    this.updateGraphSeriesAndOptions(sliceOfGraphDataByCurrentHour);
    for (currentHour; currentHour > defaultSliceTime; currentHour--) {
      this.leftShiftIteractions.push(currentHour - 1);
    }
  }

  goEnd() {
    const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour().data;
    this.leftShiftIteractions = [];
    this.updateGraphSeriesAndOptions(sliceOfGraphDataByCurrentHour);
  }

  applyFilters() {
    this.defaultGraphInterval = this.form.get('select').value;
    this.localStorageFilters.interval = this.form.get('select').value;
    this.userBehaviourLicenseUsageByIdentifierGadgetService.setLocalStorage(this.localStorageFilters);
    this.refreshGraphData(this.localStorageFilters.advancedFilter);
    this.graphIntervalRefresh();
  }

  private updateGraphSeriesAndOptions(sliceOfGraphDataByCurrentHour: { x: string; y: number; id: string; }[]) {
    this.chart.updateSeries([{
      data: sliceOfGraphDataByCurrentHour
    }]);
    this.chart.updateOptions({
      tooltip: graphTooltip(sliceOfGraphDataByCurrentHour, this.translateService.instant('dashBoard.licenseUsage.title'),
        this.translateService.instant('dashBoard.licenseUsage.hour'))
    });
  }

  private refreshGraphData(advancedFilter?: string) {
    this.insertDayPeriods();
    this.subs['request_usage_interval'] = this.userBehaviourLicenseUsageByIdentifierGadgetService.getUsageCount(advancedFilter).subscribe((res: LicensesInUsageCountOutput[]) => {
      res.forEach(element => {
        const startTime = new Date(element.startInterval);
        const endTime = new Date(element.endInterval);
        this.insertInterval(startTime, endTime, element.usageCount);
      });
      const currentHour = this.leftShiftIteractions.length > 0 ? this.leftShiftIteractions[this.leftShiftIteractions.length - 1] : new Date().getHours();
      const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour(currentHour).data;
      this.chart.updateSeries([{
        data: sliceOfGraphDataByCurrentHour
      }]);
      this.chart.updateOptions({
        tooltip: graphTooltip(sliceOfGraphDataByCurrentHour, this.translateService.instant('dashBoard.licenseUsage.title'),
          this.translateService.instant('dashBoard.licenseUsage.hour'))
      });
    });
  }

  private getSliceOfGraphDataByCurrentHour(finalHour = new Date().getHours()) {
    let sliceOfGraphDataByCurrentHour: {
      name: string;
      data: { x: string; y: number; id: string; }[]
    } = { name: this.translateService.instant('dashBoard.licenseUsage.title'), data: [] };
    finalHour = finalHour < 0 ? 0 : finalHour;
    let minutoAtual = new Date().getMinutes() % minorMinuteInterval === 0 ? new Date().getMinutes() :
      ((new Date().getMinutes() - new Date().getMinutes() % minorMinuteInterval));
    minutoAtual = minutoAtual === 60 ? 55 : minutoAtual;
    const startHour = (finalHour - defaultSliceTime) > 0 ? (finalHour - defaultSliceTime) : 0;
    const normalizedCurrentMinute = getNormalizedTime(minutoAtual);
    const normalizedStartHour = startHour < 10 ? ("0" + startHour.toString() + ":" + "00") : (startHour.toString() + ":" + "00");
    const normalizedEndHour = finalHour < 10 ? ("0" + finalHour.toString() + ":" + normalizedCurrentMinute) : (finalHour.toString() + ":" + normalizedCurrentMinute);

    const startSliceIndex = this.graphData.data.findIndex(l => l.id === normalizedStartHour);
    const endSliceIndex = this.graphData.data.findIndex(l => l.id === normalizedEndHour);

    this.disableLeftNavigation = startSliceIndex === 0;

    if (startSliceIndex != -1 && endSliceIndex != -1) {
      const endSlice = (this.graphData.data.length - 1) <= (endSliceIndex + 1) ? this.graphData.data.length : endSliceIndex + 1;
      sliceOfGraphDataByCurrentHour.data = this.graphData.data.slice(startSliceIndex, endSlice);
      return sliceOfGraphDataByCurrentHour;
    }
    return this.graphData;
  }

  private constructGraph() {
    const sliceOfGraphDataByCurrentHour = this.getSliceOfGraphDataByCurrentHour();
    this.chartOptions = constructGraph(sliceOfGraphDataByCurrentHour,
      this.translateService.instant('dashBoard.licenseUsage.title'),
      this.translateService.instant('dashBoard.licenseUsage.hour')
    );
  }

  run(): void {
    this.initializeRunState(true);
    this.updateData(null);
  }

  stop(): void {
    this.setStopState(false);
  }

  updateProperties(updatedProperties: any): void {
  }

  updateData(data: any[]): void {
  }

  preRun(): void {
    this.run();
  }

}