import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexPlotOptions,
  ApexTitleSubtitle,
  ApexTooltip,
} from 'ng-apexcharts';

export class ChartOptions {
    series: ApexAxisChartSeries;
    chart: ApexChart;
    xaxis: ApexXAxis;
    dataLabels: ApexDataLabels;
    plotOptions: ApexPlotOptions;
    title: ApexTitleSubtitle;
    tooltip: ApexTooltip;
}
