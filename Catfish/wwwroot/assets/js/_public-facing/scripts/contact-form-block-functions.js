/**
 * Submits the contact form
 * @param {any} subject
 * @param {any} recipient
 */

$(function () {
    $("#contact-form").on("submit", function (e) {
        var cfName = $("input[name='cf-name']").val();
        var email = $("input[name='cf-email']").val();
        if (!ValidateEmail(email)) {
            $("input[name='cf-email']").focus(function () {
                (this).blur();
            });
            return;
        }
        var body = $("textarea[name='cf-body']").val();

        var subject = document.getElementById('subject-value').innerText;
        var recipient = document.getElementById('recipient-value').innerText;

        var Email = { FromEmail: email, Body: body, UserName: cfName, Subject: subject, RecipientEmail: recipient }

        var url = "../../Cms/SendEmail";
        $.ajax({
            type: "POST",
            url: url,
            data: { email: Email },
            success: function (e) {
                $("#contact-form").remove();
                $('.contactForm').parent().css('align-items', 'unset');
                $('.contactForm').append("<p>Thank you for contacting us.</p>")
                    .append("<p>Your message has been sent.</p>");
                $('.contactForm').children().first().css({ 'margin-bottom': 'unset', 'font-weight': 'bold' });
    

            },
            error: function (e) {
                alert("Error while sending message.");
            }
        });
    });
});

//$('#contact-form').submit(function (event) {
//    console.log("run please");
//    //var cfName = $("input[name='cf-name']").val();
//    //var email = $("input[name='cf-email']").val();
//    //if (!ValidateEmail(email)) {
//    //    $("input[name='cf-email']").focus(function () {
//    //        (this).blur();
//    //    });
//    //    return;
//    //}
//    //var body = $("textarea[name='cf-body']").val();

//    //var Email = { FromEmail: email, Body: body, UserName: cfName, Subject: subject, RecipientEmail: recipient }

//    //var url = "../../Cms/SendEmail";
//    //$.ajax({
//    //    type: "POST",
//    //    url: url,
//    //    data: { email: Email },
//    //    success: function (e) {
//    //        alert("Thank you for contacting us.");
//    //    },
//    //    error: function (e) {
//    //        alert("Error while sending email.");
//    //    }
//    //});
//    //return false;

//});

function ValidateEmail(email) {
    //var regex = /^([a-zA-Z0-9_.+-])+\@@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,24}))$/
    return regex.test(email);
}