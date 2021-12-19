//TODO - switch to using BootstrapVue to avoid using Jquery for future features
jQuery(function () {
    //createPopUp();

    // Open modal
    jQuery(document).on("click touch", ".card-image > button", function () {
        //var elm = jQuery(this);	
        let target = $(this).attr("data-target"); //get the data-target attribute
        jQuery(target).modal();
    });

    jQuery(document).on("click touch", ".card-body > button", function () {
        //var elm = jQuery(this);	
        let target = $(this).attr("data-target"); //get the data-target attribute
        jQuery(target).modal();
    });

});