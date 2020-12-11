/*
 * DO NOT put any javascript code here that are generally needed for sites to operate.
 * This file is dedicated to put any site-specific javascript codes.
 */

/* This is for the Arc website vvv */
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

for(let i = 0; i < columns.length; i++){
  columns[i].classList.add('grey-bg');
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
        if( (i + j) % 2 != 0){
          columns[i].children[j].classList.add('col-md-5');
        }else{
          columns[i].children[j].classList.add('col-md-7');
        }
     }
  }
}

});


//this code goes in the header of Home page
document.addEventListener("DOMContentLoaded", function(event) {
    document.getElementsByClassName("footer-arc-text")[0].classList.remove('col-6');
    document.getElementsByClassName("footer-arc-text")[0].classList.add('col-md-12');
    [...document.getElementsByClassName("footer-arc-logo")].map(n => n && n.remove());
});
 */