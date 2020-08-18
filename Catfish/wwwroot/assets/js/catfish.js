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
