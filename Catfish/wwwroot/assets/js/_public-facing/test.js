if (document.getElementById("keywords-search-block-public")) {
    var vueApp = new Vue({
        el: '#keywords-search-block-public',
        data: {
            count: 0
        },
        created() {
            console.log("vue code running");
        }
    })
} else {
    console.log("can't find the element");
}