function checkEmail() {
    var email = $('#Email').val();

    $.ajax({
        url: 'Signup/EmailCheck',
        type: 'GET',
        data: { email: email },
        success: function (data) {
            if (!data) {
                $("#Email")[0].setAttribute("data-bs-original-title", "Email already exists or not a valid email");
                $("#Email").removeClass("is-valid")
                $("#Email").addClass("is-invalid")
                $('#email-error').text("Email already exists or not a valid email");
                document.getElementById('signup').disabled = true;
            } else {
                document.getElementById('signup').disabled = false;
                $("#Email").removeClass("is-invalid")
                $("#Email").addClass("is-valid")
                $('#email-error').text('');
            }
        },
        error: function () {
            console.log('Error checking email.');
        }
    });
}