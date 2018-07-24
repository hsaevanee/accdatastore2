function selectCouncil(node) {
    var field = document.getElementById('council-selection');
    field.value = node;
    field.parentElement.parentElement.parentElement.submit();
}