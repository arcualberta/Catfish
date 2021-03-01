/*
 * DO NOT put any javascript code here that are generally needed for sites to operate.
 * This file is dedicated to put any site-specific javascript codes.
 */

/* This is for the Arc website HOME PAGE */
/*
document.addEventListener("DOMContentLoaded", function(event) {
//adds yellow highlight to carousel titles
let titles = document.getElementsByClassName('title-text');
for(let title of titles){
    title.classList.add('EGTitle');
}

//changes the columns under the wide image into col-md-4's, from col-md-3's
//fixes the layout of the columns in 'Highlights'
let columns = document.getElementsByClassName('row columnBlock CBflex-container');
console.log(columns, columns.length);

for(let i = 0; i < columns.length - 1; i++){
  columns[i].classList.add('grey-bg');
  if(i == 2){
    //last columns get reversal class for mobile views
    columns[i].classList.add('reverse-columns-mobile');
  }
  for(let j = 0; j < columns[i].children.length; j++){
    if(i == 0){
        //this is the columns section under the wide image
        columns[i].children[j].classList.remove('col-md-3');
        columns[i].children[j].classList.add('col-md-4');
    }else{
        columns[i].children[j].children[0].classList.remove('col-md-12');
        columns[i].children[j].classList.remove('col-md-6');
        columns[i].children[j].classList.add('remove-padding-in-highlights');
        columns[i].children[j].classList.add('added-height');
        if(j == 0){
          //on mobile views, remove the space between the image and the text

        }

        if( (i + j) % 2 != 0){
          columns[i].children[j].classList.add('col-md-5');
          columns[i].children[j].classList.add('no-bottom-padding');
        }else{
          columns[i].children[j].classList.add('col-md-7');
        }
     }
  }
}
//for Contact Us section
columns[columns.length - 1].classList.add('space-above');
columns[columns.length - 1].children[0].classList.add('no-padding');
columns[columns.length - 1].children[0].children[0].classList.add('contact-us-column');

});


//this code goes in the HEADER of Home page
document.addEventListener("DOMContentLoaded", function(event) {
    document.getElementsByClassName("footer-arc-text")[0].classList.remove('col-6');
    document.getElementsByClassName("footer-arc-text")[0].classList.add('col-md-12');
    [...document.getElementsByClassName("footer-arc-logo")].map(n => n && n.remove());
});



//this code is for the EXAMPLE WORK PAGE ie Research Computing
document.addEventListener("DOMContentLoaded", function(event) {
//changes the columns in Meet Our Team into col-md-4's, from col-md-3's
let columns = document.getElementsByClassName('row columnBlock CBflex-container');

for(let i = 0; i < columns.length; i++){
  columns[i].style.marginBottom = "20px";
    columns[i].classList.add('box-shadow');
    columns[i].classList.add('white-background-only');
    if(i % 2 == 0){
          columns[i].classList.add('reverse-columns-mobile');
    }
  for(let j = 0; j < columns[i].children.length; j++){
    columns[i].children[j].children[0].classList.add('no-box-shadow');
    columns[i].children[j].children[0].classList.remove('col-md-12');
    columns[i].children[j].classList.add('remove-padding-in-highlights');
    columns[i].children[j].classList.add('remove-bottom-padding');
    if( (j != columns[i].children.length -1) ){
      columns[i].children[j].classList.add('added-height');
    }
    if( (i == columns.length - 1) ){
      columns[i].children[j].children[0].classList.add('contact-us-column');
    }
  }
}

columns[0].children[0].children[0].style.backgroundColor = "unset";

});

/* For the OUR TEAM page */
/*
document.addEventListener("DOMContentLoaded", function(event) {
//changes the columns in Meet Our Team into col-md-4's, from col-md-3's
let columns = document.getElementsByClassName('row columnBlock CBflex-container');

columns[0].classList.add('grey-bg');
columns[0].classList.add('box-shadow');
columns[0].classList.add('white-background');

//for the Our Partners section
columns[columns.length-1].classList.add('box-shadow');
columns[columns.length-1].classList.add('white-background');
columns[columns.length -1].style.marginTop = "20px";
columns[columns.length -1].style.marginBottom = "20px";


for(let i = 0; i < columns.length; i++){
  for(let j = 0; j < columns[i].children.length; j++){
    columns[i].children[j].classList.remove('col-md-3');
    //if it the title is inside Our Partners, make it full-sized col-12 and remove the grey bg
    if(i == 1 && j == 0){
      columns[i].children[j].classList.add('col-md-12');
      columns[i].children[j].children[0].children[0].classList.remove('grey-bg');
    }else{
      columns[i].children[j].classList.add('col-md-4');
    }
    //some custom resizing of Cybera, IST, and WestGrid
    if( (i == 1) && (j == 2 || j ==4 || j == 6) ){
console.log("here");
      columns[i].children[j].children[0].children[0].style.maxWidth = "75%";
    }

    columns[i].children[j].children[0].classList.add('no-box-shadow');
    columns[i].children[j].children[0].classList.add('column-container');
    columns[i].children[j].children[0].classList.remove('col-md-12');
  }
}

});

//this is for the OUR WORK page
document.addEventListener("DOMContentLoaded", function(event) {
//changes the background of the keywords block to grey
let keywordParent = document.getElementsByClassName('row custom-keywords');
keywordParent[0].classList.add('grey-bg');
keywordParent[0].children[0].classList.add('box-shadow');
keywordParent[0].children[0].classList.add('white-background');
keywordParent[0].children[0].classList.add('add-full-flexbox');

//adds styles for checkboxes part
let checkboxGroup = document.getElementsByClassName('checkboxgroup');
checkboxGroup[0].children[0].classList.remove('row');
let categoriesSection = document.getElementsByClassName('categories-section');
categoriesSection[0].classList.add('flex-column-no-align');
let blockTitle = document.getElementsByClassName('block-title');
blockTitle[0].classList.remove('container');
let formPart = document.getElementById('controlledVocabularySearchForm');
formPart.children[0].classList.remove('container');

//changes search result header to regular div to match the one on the left
let newDiv = document.createElement("div");
newDiv.innerHTML = "Search Results";
let searchResultsParent = document.getElementsByClassName('search-header-holder');
searchResultsParent[0].children[0].replaceWith(newDiv);
});




//this is for the CONTACT US page
document.addEventListener("DOMContentLoaded", function(event) {
let columns = document.getElementsByClassName('row columnBlock CBflex-container');

columns[0].classList.add('grey-bg');
columns[0].classList.add('box-shadow');
columns[0].classList.add('white-background');
columns[0].style.marginTop = "20px";
columns[0].style.marginBottom = "20px";

for(let i = 0; i < columns.length; i++){
  for(let j = 0; j < columns[i].children.length; j++){
    columns[i].children[j].children[0].classList.add('no-box-shadow');
    columns[i].children[j].children[0].classList.add('column-container');
    if(j == 1){
      columns[i].children[j].children[0].style.alignItems = "unset";
    }else if(i == 1){
      //this is for the map
      columns[i].children[j].classList.remove('col-md-12');
      columns[i].children[j].classList.remove('CBflex-parent');
      //for scaling up the map for mobile view
      columns[i].classList.add('map-container');
      columns[i].children[j].children[0].children[0].classList.add('map-image');
      columns[i].children[j].style.marginBottom = "-1px";
      columns[i].children[j].children[0].style.paddingLeft = "0";
      columns[i].children[j].children[0].style.paddingRight = "0";
    }
  }
}

//make contact form button left align, not right
let table = document.getElementsByTagName("tbody")[0];
table.children[4].children[1].style.textAlign = "unset";
table.children[4].children[1].children[0].value = "Submit";

});

 */