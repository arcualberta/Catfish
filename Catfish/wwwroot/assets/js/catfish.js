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
                //str1.toUpperCase() === str2.toUpperCase(); 
                if (subItem.title.toUpperCase() == menuTitle.toUpperCase()) {
                    bc = [{ label: item.title, link: item.permalink }, { label: subItem.title, link: subItem.permalink }];
                    callback(bc);
                }
            });
        } else {
            if (item.title.toUpperCase() == menuTitle.toUpperCase()) {

                bc = [{ label: item.title, link: item.permalink }]
               
                callback(bc);
            }
        }
    });
}

function updateBreadcrumb(){
    let _href = window.location.href;
    let urls = _href.split("/");
    let $thisText = (urls[urls.length - 1]).replace(/-/g, " ");//replace "-" with " "
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