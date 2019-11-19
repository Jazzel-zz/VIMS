(function ($) {
    'use strict';
    $(function () {
        
        if ($("#statistics-chart-dark").length) {
            var areaData = {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug"],
                datasets: [{
                    data: [0, 185, 75, 150, 100, 150, 50, 100],
                    backgroundColor: [
                        'rgba(108, 80, 243, .78)'
                    ],
                    borderColor: [
                        'rgba(108, 80, 243, .78)'
                    ],
                    borderWidth: 1,
                    fill: 'origin',
                    label: "purchases"
                },
                {
                    data: [0, 100, 160, 100, 150, 75, 200, 50],
                    backgroundColor: [
                        'rgba(255, 255, 255, .3)'
                    ],
                    borderColor: [
                        'rgba(255, 255, 255, .3)'
                    ],
                    borderWidth: 1,
                    fill: 'origin',
                    label: "services"
                }
                ]
            };
            var areaOptions = {
                responsive: true,
                maintainAspectRatio: true,
                plugins: {
                    filler: {
                        propagate: false
                    }
                },
                scales: {
                    xAxes: [{
                        display: true,
                        ticks: {
                            display: true
                        },
                        gridLines: {
                            display: false,
                            drawBorder: false,
                            color: 'transparent',
                            zeroLineColor: '#eeeeee'
                        }
                    }],
                    yAxes: [{
                        display: true,
                        ticks: {
                            display: true,
                            autoSkip: false,
                            maxRotation: 0,
                            stepSize: 50,
                            min: 0,
                            max: 250
                        },
                        gridLines: {
                            color: '#4a4a4a'
                        }
                    }]
                },
                legend: {
                    display: false
                },
                tooltips: {
                    enabled: true
                },
                elements: {
                    line: {
                        tension: .25
                    },
                    point: {
                        radius: 0
                    }
                }
            }
            var salesChartCanvas = $("#statistics-chart-dark").get(0).getContext("2d");
            var salesChart = new Chart(salesChartCanvas, {
                type: 'line',
                data: areaData,
                options: areaOptions
            });
            document.getElementById('statistics-legend').innerHTML = salesChart.generateLegend();
        }
        if ($("#analysis-chart").length) {
            var CurrentChartCanvas = $("#analysis-chart").get(0).getContext("2d");
            var CurrentChart = new Chart(CurrentChartCanvas, {
                type: 'bar',
                data: {
                    labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
                    datasets: [{
                        label: 'Profit',
                        data: [280, 330, 370, 410, 290, 400, 309, 530, 340, 420, 380, 240],
                        backgroundColor: '#44519e'
                    },
                    {
                        label: 'Target',
                        data: [380, 540, 600, 480, 370, 500, 450, 590, 540, 480, 510, 300],
                        backgroundColor: '#e7eaed'
                    }
                    ]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: true,
                    layout: {
                        padding: {
                            left: 0,
                            right: 0,
                            top: 20,
                            bottom: 0
                        }
                    },
                    scales: {
                        yAxes: [{
                            display: true,
                            gridLines: {
                                drawBorder: false
                            },
                            ticks: {
                                display: false
                            }
                        }],
                        xAxes: [{
                            stacked: true,
                            ticks: {
                                beginAtZero: true,
                                fontColor: "#9fa0a2"
                            },
                            gridLines: {
                                color: "rgba(0, 0, 0, 0)",
                                display: true
                            },
                            barPercentage: 0.8
                        }]
                    },
                    legend: {
                        display: false
                    },
                    elements: {
                        point: {
                            radius: 0
                        }
                    }
                }
            });
        }
    });
})(jQuery);