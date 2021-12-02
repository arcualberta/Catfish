
<script lang="ts">
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "FormEditor",
        props: ['title', 'form'],
        setup() {

        }

    });

</script>

<template>
    <h4>{{title}}</h4>
    <div>{{form}}</div>
</template>