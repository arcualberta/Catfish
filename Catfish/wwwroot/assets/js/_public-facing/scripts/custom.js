/*
 * DO NOT put any javascript code here that are generally needed for sites to operate.
 * This file is dedicated to put any site-specific javascript codes.
 */

/* This is for the Arc website vvv */
/*
//fixes the layout of the columns in 'Highlights'
document.addEventListener("DOMContentLoaded", function(event) {
let columns = document.getElementsByClassName('row columnBlock CBflex-container');
console.log(columns, columns.length);

for(let i = 1; i < columns.length; i++){
  for(let j = 0; j < columns[i].children.length; j++){
    columns[i].children[j].children[0].classList.remove('col-md-12');
    columns[i].children[j].classList.remove('col-md-6');
    columns[i].children[j].classList.add('remove-padding-in-highlights');
    if( (i + j) % 2 != 0){
      columns[i].children[j].classList.add('col-md-5');
    }else{
      columns[i].children[j].classList.add('col-md-7');
    }
  }
}

});
 */