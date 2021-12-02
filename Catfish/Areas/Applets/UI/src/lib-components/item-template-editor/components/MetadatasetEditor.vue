
<script lang="ts">
    import { defineComponent} from "vue";

    export default defineComponent({
        name: "MetadatasetEditor",
        props: ['metadataset'],
        setup() {

        }

    });

</script>

<template>
    <h4>{{metadataset.name.concatenatedContent}}</h4>
    <div>{{metadataset}}</div>
</template>