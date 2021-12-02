
<script lang="ts">
    import { defineComponent} from "vue";

    export default defineComponent({
        name: "NotificationEditor",
        props: ['title', 'container'],
        setup() {

        }

    });

</script>

<template>
    <h4>{{title}}</h4>
    <div>{{container}}</div>
</template>