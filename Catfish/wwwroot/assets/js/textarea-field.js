//register global Vue component
Vue.component("textarea-field", {
    props: ["uid", "toolbar", "model", "meta"],
    template:
        "<textarea class='form-control' " +
        "    :placeholder='meta.placeholder' " +
        "    v-model='model.value'>" +
        "</textarea>"
});

