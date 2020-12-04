/**
 * Submits the contact form
 * @param {any} subject
 * @param {any} recipient
 */

function submitForm(subject, recipient) {
    var cfName = $("input[name='cf-name']").val();
    var email = $("input[name='cf-email']").val();
    if (!ValidateEmail(email)) {
        $("input[name='cf-email']").focus(function () {
            (this).blur();
        });
        return;
    }
    var body = $("textarea[name='cf-body']").val();

    var Email = { FromEmail: email, Body: body, UserName: cfName, Subject: subject, RecipientEmail: recipient }

    var url = "../../Cms/SendEmail";
    $.ajax({
        type: "POST",
        url: url,
        data: { email: Email },
        success: function (e) {
            alert("Thank you for contacting us.");
        },
        error: function (e) {
            alert("Error while sending email.");
        }
    });

};
function ValidateEmail(email) {
    //var regex = /^([a-zA-Z0-9_.+-])+\@@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/
    return regex.test(email);
}