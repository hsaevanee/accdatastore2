$(document).ready(function () {
    $('#datatable').DataTable({
        dom: 'Bfrtip',
        paging: false,
        "aaSorting": [[1, "asc"]],
        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            },
            {
                extend: 'excelHtml5',              
            },
            {
                extend: 'csvHtml5',               
            },
            {
                extend: 'pdfHtml5',
                message: 'PDF created by PDFMake with Buttons for DataTables.',
                exportOptions: {
                        columns: [ 0,1,2,3,4,5,6,7 ]
                }
            },
            {
                extend: 'print',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                }
            }
        ],
        "columnDefs": [
            { "visible": false, "targets": [8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, , 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55] }
        ]
    });
});

function exportPDF(schoolname, levercategory) {
    //var pupilsList = @(Html.Raw(Json.Encode(data)));

    var JSONObject = {
        "schoolname": schoolname,
        "levercategory": levercategory
    }

    $.ajax({
        type: "POST",
        url: sContextPath + "DatahubProfile/IndexDatahub/getJsonPupilList",
        data: JSON.stringify(JSONObject),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            BuildPDF(data)
        },
        error: function (xhr, err) {
            SetErrorMessage(xhr);
        }
    });
}

function BuildPDF(data) {
    var dd = {
        content: [
             { text: 'This is a header', style: 'header' },
             'No styling here, this is a standard paragraph',
             { text: 'Another text', style: 'anotherStyle' },
             { text: 'Multiple styles applied', style: ['header', 'anotherStyle'] }
        ],

        styles: {
            header: {
                fontSize: 22,
                bold: true
            },
            anotherStyle: {
                italic: true,
                alignment: 'right'
            }
        }
    }

    pdfMake.createPdf(dd).open();
}