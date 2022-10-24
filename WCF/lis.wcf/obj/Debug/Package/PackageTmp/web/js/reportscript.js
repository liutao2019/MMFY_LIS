
function drawChart(div, title, subtitle, categories, data, yunit){
    return new Highcharts.Chart({
        chart: {
            renderTo: div,
           type: 'line',
           marginRight: 130,
           marginBottom: 25
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
               categories: categories
           },
           yAxis: {
               title: {
                   text: yunit
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }]
           },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>'+
                       this.x +': '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               align: 'right',
               verticalAlign: 'top',
               x: -10,
               y: 100,
               borderWidth: 0
           },
           series: [{
               name: yunit,
               data: data
           }]
    });
}

function _drawChart2(div, title, subtitle, categories, yname, yname2, data, data2, ytitle, yunit){
    return new Highcharts.Chart({
        //colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(240, 240, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            //plotBackgroundColor: 'rgba(255, 255, 255, .9)',
            //plotBorderColor: '#CCCCCC',
            //plotShadow: true,
            //plotBorderWidth: 1,
           renderTo: div,
           type: 'line',
           marginRight: 15,
           marginBottom: 30
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-20,
                        step:2,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                lineColor: '#000000',
                lineWidth: 1,
                tickWidth: 1,
                tickColor: '#000000',
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true
                    },
               min:0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            enabled:true
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>'+
                       this.x +' : '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               backgroundColor: '#FFFFFF',
               align: 'left',
               verticalAlign: 'top',
               x: 100,
               y: 0,
               floating: true,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname,
               data: data
           },{
                name: yname2,
                data: data2
           }]
    });
}

function drawChart2(div, title, subtitle, categories, yname, yname2, data, data2, ytitle, yunit){
    return new Highcharts.Chart({
        //colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4'],
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(240, 240, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            //plotBackgroundColor: 'rgba(255, 255, 255, .9)',
            //plotBorderColor: '#CCCCCC',
            //plotShadow: true,
            //plotBorderWidth: 1,
           renderTo: div,
           type: 'line',
           marginRight: 15,
           marginBottom: 30
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-20,
                        step:2,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                lineColor: '#000000',
                lineWidth: 1,
                tickWidth: 1,
                tickColor: '#000000',
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true
                    },
               min:0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            enabled:true
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>'+
                       this.x +' : '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: true,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname,
               data: data
           }]
    });
}

function drawChart3(div, title, subtitle, categories, yname, data, ytitle, yunit){
    return new Highcharts.Chart({
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(240, 240, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            renderTo: div,
           type: 'line',
           marginRight: 15,
           marginBottom: 25
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
                gridLineWidth: 1,
                lineColor: '#000000',
                tickColor: '#000000',
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-20,
                        step:2,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                tickWidth: 1,
                tickColor: '#000000',
                lineColor: '#000000',
                lineWidth: 1,
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true
                    },
               min:0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            enabled:true
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>['+
                       this.x +']: '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: true,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname,
               data: data
           }]
    });
}

function drawChart4(div, title, subtitle, categories, yname, data, ytitle, yunit){
    return new Highcharts.Chart({
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(240, 240, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            renderTo: div,
           type: 'line',
           marginRight: 15,
           marginBottom: 25
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
                gridLineWidth: 1,
                lineColor: '#000000',
                tickColor: '#000000',
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-20,
                        step:2,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                tickWidth: 1,
                tickColor: '#000000',
                lineColor: '#000000',
                lineWidth: 1,
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true
                    },
               min:0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            enabled:true
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>['+
                       this.x +']: '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: true,//
               shadow: true,//
               enabled:false,
               borderWidth: 0
               
           },
           series: [{
               name: yname,
               data: data
           }]
    });
}


function couponDrawChart(div, title, subtitle, categories, yname, data, ytitle, yunit){
    return new Highcharts.Chart({
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(240, 240, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            renderTo: div,
           type: 'line',
           marginRight: 15,
           marginBottom: 25
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
                gridLineWidth: 1,
                lineColor: '#000000',
                tickColor: '#000000',
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-20,
                        step:1,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                tickWidth: 1,
                tickColor: '#000000',
                lineColor: '#000000',
                lineWidth: 1,
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true
                    },
               min:0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            enabled:true
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>['+
                       this.x +']: '+ this.y +'' + yunit;
               }
           },
           legend: {
               layout: 'vertical',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: true,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname,
               data: data
           }]
    });
}

function rateDrawChart(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,para1){
    return new Highcharts.Chart({
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(255, 255, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            renderTo: div,
           type: 'line',
           shadow:true,
           marginRight: 15,
           marginBottom: 30
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
                gridLineWidth: 1,
                lineColor: '#000000',
                tickColor: '#000000',
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-15,
                        step:1,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                tickWidth: 1,
                tickColor: '#000000',
                lineColor: '#000000',
                lineWidth: 1,
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true,
                        formatter:function(){
                            return this.value.toFixed(5);
                        }
                    },
               tickInterval:para1
               //min:1.00000,
               //max:5.0
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            //enabled:true
                            enabled:false
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>['+
                       this.x +']: '+ this.y +'' + yunit;
               }
           },
           legend: {
               //layout: 'vertical',
               layout: 'horizontal',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: false,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname_c,
               data: data_c
           },{
               name: yname_s,
               data: data_s
           },{
               name: yname_d,
               data: data_d
           }]
    });
}

function rateDrawChartB(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,para1,para2,para3){
    return new Highcharts.Chart({
        chart: {
            backgroundColor: {
                 linearGradient: [0, 0, 500, 500],
                 stops: [
                    [0, 'rgb(255, 255, 255)'],
                    [1, 'rgb(255, 255, 255)']
                 ]
            },
            renderTo: div,
           type: 'line',
           shadow:true,
           marginRight: 15,
           marginBottom: 30
        },
           title: {
               text: title,
           x: -20 //center
           },
           subtitle: {
               text: subtitle,
           x: -20
           },
           xAxis: {
                gridLineWidth: 1,
                lineColor: '#000000',
                tickColor: '#000000',
               categories: categories,
               labels:{
                        enabled:true,
                        rotation:-15,
                        step:1,
                        align:'right'
                    }
           },
           yAxis: {
                minorTickInterval: 'auto',
                tickWidth: 1,
                tickColor: '#000000',
                lineColor: '#000000',
                lineWidth: 1,
               title: {
                   text: ytitle
               },
               plotLines: [{
                   value: 0,
                   width: 1,
                   color: '#808080'
               }],
               labels:{
                        enabled:true,
                        formatter:function(){
                            return this.value.toFixed(5);
                        }
                    },
                    //minorTickInterval:4,
               tickInterval:para1,
               min:para2,
               max:para3
           },
           plotOptions:{
                    line:{
                        dataLabels:{
                            //enabled:true
                            enabled:false
                        }
                    }
                },
           tooltip: {
               formatter: function() {
                   return '<b>'+ this.series.name +'</b><br/>['+
                       this.x +']: '+ this.y +'' + yunit;
               }
           },
           legend: {
               //layout: 'vertical',
               layout: 'horizontal',
               backgroundColor: '#FFFFFF',
               align: 'right',
               verticalAlign: 'top',
               x: 0,
               y: 0,
               floating: false,//
               shadow: true,//
               borderWidth: 0
           },
           series: [{
               name: yname_c,
               data: data_c
           },{
               name: yname_s,
               data: data_s
           },{
               name: yname_d,
               data: data_d
           }]
    });
}


function drawGlobalR(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}
function drawGlobalJ(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}
function drawGlobalZ(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}

function drawItemR(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}
function drawItemJ(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}
function drawItemZ(div, categories, data){
    drawChart(div, '', '', categories, data, '');
}

function loadChart(div, url){
    $.getJSON(url, function(json){
        var title = json.title;
        var subtitle = json.subtitle;
        var yunit = json.yunit;
        var categories = json.categories;
        var data = json.data;
        drawChart(div, title, subtitle, categories, data, yunit);
    });
}

function loadChart2(div, url){
    $.getJSON(url, function(json){
        var title = json.title;
        var subtitle = json.subtitle;
        var categories = json.categories;
        var yname = json.yname;
        var data = json.data;
        var yname2 = json.yname2;
        var data2 = json.data2;
        var ytitle = json.ytitle;
        var yunit = json.yunit;
        drawChart2(div, title, subtitle, categories, yname, yname2, data, data2, ytitle, yunit);
    });
}

function loadChart3(div, url){
    $.getJSON(url, function(json){
        var title = json.title;
        var subtitle = json.subtitle;
        var categories = json.categories;
        var yname = json.yname;
        var data = json.data;
        var ytitle = json.ytitle;
        var yunit = json.yunit;
        drawChart3(div, title, subtitle, categories, yname, data, ytitle, yunit);
    });
}

function loadChartRate(div, url){
    $.getJSON(url, function(json){
        var title = json.title;
        var subtitle = json.subtitle;
        var categories = json.categories;
        var yname_s = json.yname_s;
        var data_s = json.data_s;
        var yname_c = json.yname_c;
        var data_c = json.data_c;
        var yname_d = json.yname_d;
        var data_d = json.data_d;
        var ytitle = json.ytitle;
        var yunit = json.yunit;
        var sign = json.sign;
        var jg = json.jg;
        
        if(sign==1){
            //var para = 0.03555;
            //var para = jg;
            if(jg==0){
                var mypara1 = 0.25;
                var mypara2 = 1.00000;
                var mypara3 = 5.00000;
                rateDrawChartB(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,mypara1,mypara2,mypara3);
            }else{
                rateDrawChart(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,jg);
            }
        }else if(sign==2){
            var mypara1 = 0.25;
            var mypara2 = 1.00000;
            var mypara3 = 5.00000;
            rateDrawChartB(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,mypara1,mypara2,mypara3);
        }else{
            var para1 = 0.25;
            var para2 = 0.00000;
            var para3 = 5.00000;
            rateDrawChartB(div, title, subtitle, categories, yname_s, data_s, yname_c, data_c, yname_d, data_d, ytitle, yunit,para1,para2,para3);
        }
    });
}