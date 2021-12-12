<script lang="ts">
    
    import { defineComponent, PropType, ref } from 'vue'
    import { useStore } from 'vuex';

    import { AttachmentField} from '../../models/fieldContainer'

    export default defineComponent({
        name: "AttachmentField",
        components: {
           
        },
        props: {
            model: {
                type: null as PropType<AttachmentField> | null,
                required: true
            },


        },
        
        setup(p) {
            const store = useStore();
            const itemId = ref(store.state.item.id);
            
            const dataItemId = ref(store.getters.dataItemId);
            
            const fileUrl = '/api/items/' + itemId.value + '/' + dataItemId.value + '/' + p.model.id + '/';
            
            return {
                itemId,
                fileUrl
               
            }
        }
           
    });
</script>

<template>
    <div v-for="file in model.files">
        <a :href="fileUrl + file.fileName" ><img :src="file.thumbnail" class="img-thumbnail"></a>{{file.originalFileName}}
       
    </div>
</template>

<style scoped>
    .img-thumbnail{
        width:35px;
        height: auto;
        margin-right: 10px;
    }
</style>
