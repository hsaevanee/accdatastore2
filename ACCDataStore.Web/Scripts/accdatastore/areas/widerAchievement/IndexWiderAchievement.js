
$(document).ready(function () {
    $('#datatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
             {
                 extend: 'copyHtml5',
             },
             {
                 extend: 'excelHtml5',
             },
             {
                 extend: 'csvHtml5',
             },
             {
                 extend: 'pdfHtml5',
                 orientation: 'landscape',

             },
             {
                 extend: 'print',
             }
        ]
    });
});

 