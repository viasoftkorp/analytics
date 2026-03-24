import { ChartOptions } from '../../tokens/apex-chart';

export function graphTooltip(sliceOfGraphDataByCurrentHour: { x: string; y: number; id: string; }[], title: string, hour: string) {
    return {
        marker: {
            fillColors: ['#c5d4ed']
        },
        custom: ({ series, seriesIndex, dataPointIndex }) => {
            return `
          <div class="tooltip-graph">
            <p>${hour}: <b>${sliceOfGraphDataByCurrentHour[dataPointIndex].id}</b></p>
            <div>
              <p>${title}: <b>${series[seriesIndex][dataPointIndex]}</b></p>
            </div>
          </div>
          `
        }
    }
}

export function constructGraph(sliceOfGraphDataByCurrentHour: {
    name: string;
    data: { x: string; y: number; id: string; }[]
}, title: string, hour: string) {
    let options: Partial<ChartOptions>;
    options = {
        series: [
            sliceOfGraphDataByCurrentHour
        ],
        chart: {
            type: "area",
            height: 350,
            animations: {
                enabled: false
            },
            zoom: {
                enabled: false
            },
            toolbar: {
                show: false
            }
        },
        dataLabels: {
            enabled: false
        },
        markers: {
            colors: ['#576782']
        },
        stroke: {
            curve: "straight",
            colors: ['#89a2cc']
        },
        title: {
            text: title,
            align: "left",
            style: {
                fontSize: "14px",
                fontFamily: '"Open Sans", sans-serif',
                fontWeight: 500
            }
        },
        fill: {
            opacity: 0.5,
            type: ['solid'],
            colors: ['#c5d4ed']
        },
        tooltip: graphTooltip(sliceOfGraphDataByCurrentHour.data, title, hour),
        xaxis: {
            labels: {
                style: {
                    fontSize: '11px',
                    fontFamily: '"Open Sans", sans-serif'
                }
            },
            tooltip: {
                enabled: false
            }
        },
        yaxis: {
            labels: {
                style: {
                    fontSize: '11px',
                    fontFamily: '"Open Sans", sans-serif'
                }
            },
        }
    };
    return options;
}