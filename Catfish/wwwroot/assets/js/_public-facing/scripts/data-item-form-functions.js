﻿//Setting up a gloab variable to track whether data form(s) were modified
var dataFormdModified = false;

$(document).ready(function () {

    //Encoding inner-HTML of composite field templates into base 64 strings
    $.each($("form .composite-field-template"), function (idx, template) {
        let html = $(template).html();
        let encodedHtml = btoa(html);
        $(template).html(encodedHtml);
    });

    //Encoding inner-HTML of table field templates into base 64 strings
    $.each($("form .tf-template"), function (idx, template) {
        let html = $(template).html();
        let encodedHtml = btoa(html);
        $(template).html(encodedHtml);
    });

    setInterval(function () {
        if (dataFormdModified) {
            autoSaveDataItemForms();
            dataFormdModified = false;
        }
    },
    10 * 60000); //execute every 10 minutes.

});

function removeRequiredAttribute(form) {
    $(form).find("input,select,textarea").removeAttr("required");
}

function autoSaveDataItemForms() {
        let forms = $(".data-item-form");
        $.each(forms, function (index, form) {
            autoSaveForm(form);
        });
}

function autoSaveForm(form) {

    //Reguar expression for matching the variable name prefix up to the item's properties.
    var prefix = /^Blocks\[[0-9]+\]\.Item\.|^block.Item\./;
    var name;
    var values = {};

    //Handling text areas and input elements EXCLUDING checkboxes, radio buttons, and drop-down (select) menus
    $.each($('input, textarea', form).not('input[type=checkbox], input[type=radio], select').serializeArray(), function (i, field) {
        name = field.name.replace(prefix, "");
        values[name] = field.value;
    });

    //Handling checkbox sets
    $.each($('input[type=checkbox]:checked', form).serializeArray(), function (i, field) {
        name = field.name.replace(prefix, "");
        values[name] = true;
    });

    //Handling radio-button fields and drop-down menus
    $.each($('input[type=radio]:checked, select', form).serializeArray(), function (i, field) {
        name = field.name.replace(prefix, "") + ".SelectedOptionGuids";
        values[name] = field.value ? [field.value] : [];
    });


    let url = "/api/items/AutoSave";

    /* Send the data using post with element id name and name2*/
    var posting = $.post(url, values);
    var message = "";

    posting.done(function (data) {
        if (data.success) {
            //message = data.message ? data.message : "Auto-save succesful";
        }
        else {
            //message = data.message ? data.message : "Auto-save failed";
        }
    });
}