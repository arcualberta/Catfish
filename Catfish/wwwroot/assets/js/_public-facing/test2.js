if (document.getElementById("not-real-tag")) {
    var vueApp = new Vue({
        el: '#keywords-search-block-public',
        data: {
            count: 0
        },
        created() {
            console.log("vue code running but shouldnt");
        }
    })
} else {
    console.log("can't find the element which i shouldnt");
}