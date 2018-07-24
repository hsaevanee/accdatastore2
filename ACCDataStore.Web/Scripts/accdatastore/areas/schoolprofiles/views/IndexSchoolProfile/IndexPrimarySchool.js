
$(document).ready(function () {

    $('#SchoolRollDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'School Roll ' + year
                }, 'print',
            ]
        }
    });

    $('#SchoolRollForecastDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'School Roll ' + year
                }, 'print',
            ]
        }
    });


    $('table.displayAttendancetable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Attendance ' + year
                }, 'print',
            ]
        }
    });


    $('table.displayExclusiontable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Exclusions-Annual ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayFSMtable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Free School Meal Entitlement ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayPIPtable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'PIPS Baseline P1 for ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayDTTableV').DataTable({
        dom: 'Bfrtip',
        "scrollY": "380px",
        "scrollCollapse": true,
        "paging": false,
        //responsive: true,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'portrait' }, 'print',
            ]
        }
    });

    $('table.displaySIMDtable').DataTable({
        dom: 'Bfrtip',
        "scrollY": "380px",
        "scrollCollapse": true,
        "paging": false,
        //responsive: true,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', { extend: 'pdfHtml5', orientation: 'portrait' }, 'print',
            ]
        }
    });

    $('table.displayLookedAftertable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5',  'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Looked After Children ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayStagetable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'School Roll Year Groups ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayEthnictable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": "380px",
        "order": [],
        "columnDefs": [ {
            "targets"  : 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Ethnicity for ' + year
                }, 'print',
            ]
        }
    });


    $('table.displayNationalitytable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'portrait',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'Nationality for ' + year
                }, 'print',
            ]
        }
    });

    $('table.displayEnglishLeveltable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": false,
        "order": [[0, 'asc']],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'csvHtml5', {
                    extend: 'pdfHtml5',
                    orientation: 'landscape',
                    exportOptions: {
                        modifier: {
                            page: 'current'
                        }
                    },
                    header: true,
                    title: 'English Level for ' + year
                }, 'print',
            ]
        }
    })

   

    //DrawTempGraph();
    DrawEnglishLevelGraph();
    DrawNationlityGraph();
    DrawSIMDGraph();
    DrawSchoolRollForecastGraph();

    $("a[data-toggle=\"tab\"]").on("shown.bs.tab", function (e) {
        $($.fn.dataTable.tables(true)).DataTable()
          .columns.adjust()
          .responsive.recalc();
    });

    $('#englishgraph').on('shown.bs.collapse', function () {
        var chart = $('#EnglishLevelGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    })

    $('#simdgraph').on('shown.bs.collapse', function () {
        var chart = $('#SimdGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    })
    $('#simdtable').on('shown.bs.collapse', function () {
        $('#SIMDDatatable').DataTable() // target the chart itself
        .columns.adjust()
        .responsive.recalc(); // reflow that chart
    })

    $('#nationalitychart').on('shown.bs.collapse', function () {
        var chart = $('#NationalityGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    })

    $('#tabsimd').on('shown.bs.tab', function (e) {
        var chart = $('#SimdGraphContainer2').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
        chart = $('#SimdGraphContainer3').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });

    $('#tabnationality').on('shown.bs.tab', function (e) {
        var chart = $('#NationalityGraphContainer2').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
        chart = $('#NationalityGraphContainer3').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });

    $('#tabeal').on('shown.bs.tab', function (e) {
        var chart = $('#EnglishLevelGraphContainer2').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
        chart = $('#EnglishLevelGraphContainer3').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });

    $('#tabschoolroll').on('shown.bs.tab', function (e) {
        var chart = $('#SchoolRollForecastGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });


});

function DrawTempGraph() {
    var options = {
        chart: {
            renderTo: 'TempGraphContainer',
            type: 'spline'
        },
        series: [{}]
    };

    $.getJSON('jsonDataTable', function (data) {
        options.series[0].data = data;
        var chart = new Highcharts.Chart(options);
    });

}

function requestData() {
    $.ajax({
        url: 'api/v1/dashboard/month_mention_graphic',
        type: "GET",
        dataType: "json",
        data: { username: "demo" },
        success: function (data) {
            chart.addSeries({
                name: "mentions",
                data: data.month_mentions_graphic
            });
        },
        cache: false
    });
}


function DrawNationlityGraph() {
    Highcharts.setOptions({
        colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4']
    });

    var data = {
        table: 'Nationalitydatatable',
        switchRowsAndColumns: true
    };
    var chart = {
        type: 'column',
        options3d: {
            enabled: true,
            alpha: 10,
            beta: 25,
            depth: 70
        }
    };
    var title = {
        text: 'Nationality'
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: '% of pupils'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ' + this.point.name.toLowerCase();
        }
    };
    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;
    
    $('#NationalityGraphContainer').highcharts(json);
    $('#NationalityGraphContainer2').highcharts(json);
    json.data = {
        table: 'Nationalitydatatable'
    };

    $('#NationalityGraphContainer3').highcharts(json);

}


function DrawEnglishLevelGraph() {
    Highcharts.setOptions({
        colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4']
    });

    var data = {
        table: 'EnglishLevelDatatable',
        switchRowsAndColumns: true
    };
    var chart = {
        type: 'column',
        options3d: {
            enabled: true,
            alpha: 10,
            beta: 25,
            depth: 70
        }
    };
    var title = {
        text: 'Level of English'
    };
    var subtitle = {
        text: 'Graph excluded "English as a first-language" data',
        x: -20
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: '% of pupils'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ' ;
        }
    };

    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;

    var chart = $('#EnglishLevelGraphContainer').highcharts(json);
    var chart = $('#EnglishLevelGraphContainer2').highcharts(json);
    json.data  = {
        table: 'EnglishLevelDatatable',
        switchRowsAndColumns: false
    };
    var chart = $('#EnglishLevelGraphContainer3').highcharts(json);

}


function DrawSIMDGraph() {

    Highcharts.setOptions({
        colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4']
    });

    var data = {
        table: 'SIMDDatatable',
        switchRowsAndColumns: true
    };
    var chart = {
        type: 'column',
        options3d: {
            enabled: true,
            alpha: 10,
            beta: 25,
            depth: 70
        }
    };
    var title = {
        text: 'Scottish Index of Multiple Deprivation'
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: '% of pupils in Each Decile'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ';
        }
    };

    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;

    var chart = $('#SimdGraphContainer').highcharts(json);
    var chart = $('#SimdGraphContainer2').highcharts(json);
    json.data = {
        table: 'SIMDDatatable',
        switchRowsAndColumns: false
    };
    var chart = $('#SimdGraphContainer3').highcharts(json);
}

function DrawSchoolRollForecastGraph() {
    Highcharts.setOptions({
        colors: ['#058DC7', '#50B432', '#ED561B', '#DDDF00', '#24CBE5', '#64E572', '#FF9655', '#FFF263', '#6AF9C4']
    });
    var data = {
        table: 'SchoolRollForecastDatatable',
        switchRowsAndColumns: true
    };
    var chart = {
        type: 'line',
    };
    var title = {
        text: 'School Roll Forecast'
    };
    var yAxis = {
        allowDecimals: false,
        title: {
            text: 'Number of Pupils'
        }
    };
    var tooltip = {
        formatter: function () {
            return '<b>' + this.series.name + '</b><br/>' +
               this.point.y + ' ';
        }
    };

    var credits = {
        enabled: false
    };

    var json = {};
    json.chart = chart;
    json.title = title;
    json.data = data;
    json.yAxis = yAxis;
    json.credits = credits;
    json.tooltip = tooltip;

    var chart = $('#SchoolRollForecastGraphContainer').highcharts(json);
}