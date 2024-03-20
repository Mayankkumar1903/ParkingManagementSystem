//$(document).ready(function () {
//    $('#btnsubmit').click(function (e) {
//        e.preventDefault();
//        var email = $("#Email").val();
//        var password = $("#Password").val();

//        // Create a JSON object
//        var jsonData = {
//            "email": email,
//            "password": password
//        };
//        $.ajax({
//            type: "POST",
//            url: '/Login/Login',
//            data: jsonData,
//            success: function (result) {
//            },
//            error: function (xhr, status, error) {
//                console.log(xhr);
//            }
//        });
//    });
//});

//$('#loginForm').submit(function (e) {
//    var formData = $(this).serialize();
//    $.ajax({
//        type: "POST",
//        url: '/Login/Login',
//        data: formData,
//        success: function (result) {
//            console.log(result);
//        },
//        error: function (xhr, status, error) {
//        }
//    });
//});