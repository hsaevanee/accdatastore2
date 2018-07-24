function goToCreateURL(object) {    
    var sSchoolNameText = $('#selectedschoolname option:selected').text();
    return object.href += sSchoolNameText;
}

$(document).ready(function () {
    $("#selectedschoolname").change(function () {
        //alert($(this).val());
        //window.location = sContextPath + 'SchoolProfile/IndexSchoolProfile/Index/'+$(this).val();
        document.forms[0].submit();
    });
});