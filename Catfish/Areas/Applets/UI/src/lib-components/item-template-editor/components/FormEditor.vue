
<script lang="ts">
    import { defineComponent } from "vue";

    export default defineComponent({
        name: "FormEditor",
        props: ['form'],
        setup() {

        }

    });

</script>

<template>
    <h4>{{form.name.concatenatedContent}}</h4>
    <div>{{form}}</div>
</template>