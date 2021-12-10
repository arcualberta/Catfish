<script lang="ts">
    
    import { defineComponent, PropType} from 'vue'
    import { AttachmentField} from '../../models/fieldContainer'

    export default defineComponent({
        name: "AttachmentField",
        components: {
           
        },
        props: {
            model: {
                type: null as PropType<AttachmentField> | null,
                required: true
            }

        }
           
    });
</script>

<template>
    <div v-for="file in model.files">
        <img :src="file.thumbnail" class="img-thumbnail">{{file.originalFileName}}
    </div>
</template>

<style scoped>
    .img-thumbnail{
        width:35px;
        height: auto;
        margin-right: 10px;
    }
</style>
