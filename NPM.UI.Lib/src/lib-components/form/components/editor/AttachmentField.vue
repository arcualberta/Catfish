<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import * as models from '../../models'
    import DropZone from './DropZone.vue'

    export default defineComponent({
        name: "AttachmentField",
        components: {
            DropZone
        },
        props: {
            model: {
                type: null as PropType<models.AttachmentField> | null,
                required: false
            },
        },
    });
</script>


<template>
    Attachment
    <div>{{selectedFileNames?.join(' | ')}}</div>
    <DropZone :id="model.id" @drop.prevent="drop" @change="selectFiles" :key="model.id"/>
</template>
