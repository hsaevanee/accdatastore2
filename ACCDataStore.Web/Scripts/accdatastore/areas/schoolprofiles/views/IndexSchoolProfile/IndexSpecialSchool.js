
$(document).ready(function () {
    $('#AttendanceDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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


    $('#ExclusionDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#FreeMealDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#LookedAfterDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#StageDatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#Ethnicdatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": "380px",
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#Ethnicdatatable2').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "scrollY": "380px",
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#Nationalitydatatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "order": [],
        "columnDefs": [{
            "targets": 'no-sort',
            "orderable": false,
        }],
        buttons: {
            buttons: [
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#EnglishLevelDatatable').DataTable({
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
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $('#EnglishLevelDatatable2').DataTable({
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
                'copyHtml5', 'excelHtml5', 'csvHtml5', {
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

    $("a[data-toggle=\"tab\"]").on("shown.bs.tab", function (e) {
        $($.fn.dataTable.tables(true)).DataTable()
          .columns.adjust()
          .responsive.recalc();
    });

    $('#demo1').on('shown.bs.collapse', function () {
        var chart = $('#EnglishLevelGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    })

    $('#nationachart').on('shown.bs.collapse', function () {
        var chart = $('#NationalityGraphContainer').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    })

    $('#tab8').on('shown.bs.tab', function (e) {
        var chart = $('#NationalityGraphContainer2').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
        chart = $('#NationalityGraphContainer3').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });


    $('#tab6').on('shown.bs.tab', function (e) {
        var chart = $('#EnglishLevelGraphContainer2').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
        chart = $('#EnglishLevelGraphContainer3').highcharts(); // target the chart itself
        chart.reflow() // reflow that chart
    });


    //DrawTempGraph();
    DrawEnglishLevelGraph();
    DrawNationlityGraph();


});

function DrawNationlityGraph() {
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
    json.subtitle = subtitle;

    var chart = $('#EnglishLevelGraphContainer').highcharts(json);
    var chart = $('#EnglishLevelGraphContainer2').highcharts(json);
    json.data = {
        table: 'EnglishLevelDatatable',
        switchRowsAndColumns: false
    };
    var chart = $('#EnglishLevelGraphContainer3').highcharts(json);

}

