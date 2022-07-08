//This file contain generic js classes 
// USAGE: SwitchLang("en", arrayOfSupportedLanguages)
 // 
function switchLang(lang,  supportLang){
     let pathname = window.location.pathname; // Returns path only (/path/example.html)
     let url      = window.location.href;     // Returns full URL (https://example.com/path/example.html)
     let origin   = window.location.origin;
     for (var i = 0; i < supportLang.length; i++) {
         let urlLang =  pathname.substring(1,3);
         if(urlLang == "")
              break;
         if(supportLang[i] === urlLang)
         {
           pathname = pathname.substring(3);
           break;
         }
       }
     let newUrl = origin + "/" + lang + pathname;
     window.location.replace(newUrl);
}

function getBreadcrumbTrail(menuTitle, callback) {
    let bc = [];
    sitemap.forEach(function (item) {
        if (item.items.length > 0) {
            item.items.forEach(function (subItem) {
                //compare the slug instead of page title -- MR: Dec 7 2020
                if (subItem.permalink == menuTitle) {      // (subItem.title.toUpperCase() == menuTitle.toUpperCase()) {
                    //do not display as link when it has sub page -- because this parent page usually don't have any contain
                    //
                    bc = [{ label: item.title, link: "#" }, { label: subItem.title, link: subItem.permalink }]; //1 level submenu
                    callback(bc);
                }
                if (subItem.items.length > 0) {
                    getBreadCrumbTrailRecursive(subItem, menuTitle, callback, subItem.title); //sub menu 2 level down
                }
            });
        } else {
            if (item.permalink == menuTitle) { //(item.title.toUpperCase() == menuTitle.toUpperCase()) {

                bc = [{ label: item.title, link: item.permalink }]

                callback(bc);
            }
        }
    });
}
function getBreadCrumbTrailRecursive(item, menuTitle, callback, parentTitle) {
    let bc = [];
    if (item.items.length > 0) {
        item.items.forEach(function (subItem) {
            //compare the slug instead of page title -- MR: Dec 7 2020
            if (subItem.permalink == menuTitle) {      // (subItem.title.toUpperCase() == menuTitle.toUpperCase()) {
                //do not display as link when it has sub page -- because this parent page usually don't have any contain
                //
                bc = [{ label: parentTitle, link: "#" }, { label: item.title, link: item.permalink }, { label: subItem.title, link: subItem.permalink }];
                callback(bc);
                if (subItem.items.length > 0) {
                    getBreadCrumbTrailRecursive(subItem, menuTitle, callback, subItem.title)
                };
            }

        });
    }
}

function updateBreadcrumb(){
   // let _href = window.location.href;
   // let urls = _href.split("/");
    //MR Dec 7 2020 -- get the slug instead of page title
   // let $thisText = "/" + urls[urls.length - 1].trim(); 
    let $thisText = window.location.pathname.trim();
    let $bc = `<li class="breadcrumb-item"><a href="/" class="fa fa-home"></a></li>`;

    if ($thisText !== "") {
        getBreadcrumbTrail($thisText, function (bTrail) { //using callback func


            bTrail.forEach(function (itm) {

                if (itm.link === "/") {// home page
                    $bc = `<li class="breadcrumb-item"><a href="/" class="fa fa-home"></a></li>`;

                } else {
                    $bc += "<li class='breadcrumb-item'><a href='" + itm.link + "'>" + itm.label + "</a></li>";
                }
            });
        });
    }
    $('.breadcrumb').empty().append($bc)

    return false;

}
//title = string -- for fileName, tableId => <table id> that contain the data
function generateCSV(title, tableId) {
    var title = title;//$("#SelectedEventReports :selected").text().replace(/[\\\/ \.]+/g, "_");
    var report = "";
    var rows = $('#' + tableId +' > tbody > tr').each(function () {
        $(this).children('th, td').each(function (index) {
            if (index == 0) {
                return;
            }

            var text = $(this).text().replace(/"/g, '""').replace(/\n[ \t\r\v\f]+/g, '\n');

            if (index > 1) {
                report += ',';
            }

            report += '"' + text.trim() + '"';
        })

        report += "\n";
    })

    var link = document.createElement('a');
    link.download = title + '.csv';
    link.href = 'data:text/csv;charset=UTF-8,' + encodeURIComponent(report);


    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
    delete link;
}

$(function () { 
    /* show SUB Menu ==== MR July 27 2021 */
    $('.dropdown-menu a.dropdown-toggle').on('click', function (e) {
        if (!$(this).next().hasClass('show')) {
            $(this).parents('.dropdown-menu').first().find('.show').removeClass('show');
        }
        var $subMenu = $(this).next('.dropdown-menu');
        $subMenu.toggleClass('show');


        $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function (e) {
            $('.dropdown-submenu .show').removeClass('show');
        });


        return false;
    });
});
