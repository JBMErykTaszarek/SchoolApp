$(document).ready(function () {

    $.ajax({
        url: '/SchoolTask/SchoolTasksList',
        success: function (resault) {
            $('#tableDiv').html(resault);
        }
    });

});