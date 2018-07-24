
$(document).ready(function () {
  
    var table1 = $('#summaryDatatable').DataTable({
        dom: 'Bfrtip',
        "scrollY": "200px",
        "scrollCollapse": true,
        paging: false,
        "order": [],
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
                    title: 'School breakdown for Aberdeen city'
                }, 'print',
            ]
        }
    });


    var table2 = $('#summaryNeighbourhoodDatatable').DataTable({
        dom: 'Bfrtip',
        "scrollY": "200px",
        "scrollCollapse": true,
        paging: false,
        "order": [[1, 'asc']],
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
                    title: 'Neightbourhood breakdown for Aberdeen city'
                }, 'print',
            ]
        }
    });

    table1.on('order.dt search.dt', function () {
        table1.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    table2.on('order.dt search.dt', function () {
        table2.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

});

function goToCreateURL(object) {
    var code = $('#selectedschoolcode :selected').text();
    return object.href += code;
}

//function CheckFormat(number) {
//    if(number.val() <=10)
//        return number.replace("%s","*");;
//}

function FunctiongetDetail(buttonID, dataname) {
    var schcode = $('#selectedschoolcode :selected').val();

    var JSONObject = {
        "schcode": schcode,
        "dataname": dataname,

    }

        $.ajax({
            type: 'POST',
            url: sContextPath + 'DatahubProfile/IndexDatahub/GetDatadetails',
            data: JSON.stringify(JSONObject),
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                alert("getdatadetails"+data.length);
            },
            error: function (xhr, err) {
                if (xhr.readyState != 0 && xhr.status != 0) {
                    alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                    alert('responseText: ' + xhr.responseText);
                }
            }
        });
    
}

function myFunctionColumn(JSONObject) {
    $.ajax({
        type: 'POST',
        url: sContextPath + 'DatahubProfile/IndexDatahub/SearchByName',
        data: JSON.stringify(JSONObject),
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        success: function (data) {
            drawChartColumn(data);
        },
        error: function (xhr, err) {
            if (xhr.readyState != 0 && xhr.status != 0) {
                alert('readyState: ' + xhr.readyState + '\nstatus: ' + xhr.status);
                alert('responseText: ' + xhr.responseText);
            }
        }
    });


}

 