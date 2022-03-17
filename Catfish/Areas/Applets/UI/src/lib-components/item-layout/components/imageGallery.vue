<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    //import { useStore } from 'vuex';
    //import { ComponentField } from "../models/componentField"
    import { AttachmentField /*, FileReference*/} from "../../shared/models/fieldContainer"
   export default defineComponent({
            name: "ImageGallery",
       components: {
       
        },
       props: {
           model: {
               type: null as PropType<AttachmentField> | null,
               required: true
           },

           fileUrl: {
               type: null as PropType<string> | null,
               required: true
           }
       },
       setup(p) {
           
           return {
               files: computed(() => p.model.files.$values),  
               fieldUrl: computed(() => p.fileUrl),
           }
       },

      
       
    });
</script>

<template>
    <h5>Gallery</h5>
   
    <div class="container">
        <div class="d-flex flex-row">
            <img v-for="file in files" :src="fileUrl + file.fileName" class="w-100 shadow-1-strong rounded mb-4" :alt="file.originalFileName" />
        </div>   
    </div>
   
</template>

