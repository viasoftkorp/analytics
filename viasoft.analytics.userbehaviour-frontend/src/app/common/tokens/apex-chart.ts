import {
    ApexAxisChartSeries,
    ApexChart,
    ApexXAxis,
    ApexDataLabels,
    ApexTooltip,
    ApexStroke,
    ApexMarkers,
    ApexYAxis,
    ApexTheme,
    ApexTitleSubtitle,
    ApexFill
  } from "ng-apexcharts";

export type ChartOptions = {
    series: ApexAxisChartSeries;
    chart: ApexChart;
    xaxis: ApexXAxis;
    stroke: ApexStroke;
    tooltip: ApexTooltip;
    dataLabels: ApexDataLabels;
    markers: ApexMarkers;
    theme: ApexTheme;
    yaxis: ApexYAxis;
    fill: ApexFill;
    title: ApexTitleSubtitle;
  };