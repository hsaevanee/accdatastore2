
$(document).ready(function () {


    $("#selectedschoolname").change(function () {
        //alert("selectedschoolname");
        //window.location = sContextPath + 'SchoolProfile/IndexSchoolProfile/Index/'+$(this).val();
        if (validateDropdownlist()==true) {
            document.forms[0].submit();
        }        
    });

    $("#selectedschoolname2").change(function () {
        //alert("selectedschoolname2");
        //window.location = sContextPath + 'SchoolProfile/IndexSchoolProfile/Index/'+$(this).val();
        if (validateDropdownlist() == true) {
            document.forms[0].submit();
        }
    });

    $('#SIMDdatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5',   'print'
        ]
    });

    $('#Ethnicdatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#Nationalitydatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#StdStagedatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#EALdatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

    $('#FSMdatatable').DataTable({
        paging: false,
        "aaSorting": [[0, "asc"]],
        dom: 'Bfrtip',
        buttons: [
       'copyHtml5', 'excelHtml5', 'csvHtml5', 'pdfHtml5', 'print'
        ]
    });

});

function validateDropdownlist() {
    var value1 = $('#selectedschoolname :selected').text();
    var value2 = $('#selectedschoolname2 :selected').text();

    if (value1 == "---Please Select School---" && value2 == "---Please Select School---") {
        alert('Please select School');
        return false;
    } else {
        return true;
    }

}