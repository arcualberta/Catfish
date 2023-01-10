<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    //import { useStore } from 'vuex';
    //import { ComponentField } from "../models/componentField"
    import { AttachmentField /*, FileReference*/} from "../../shared/models/fieldContainer"
   export default defineComponent({
            name: "ImageCarousel",
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
               file1stId: computed(() => p.model.files.$values[0].id)
           }
       },

      
       
    });
</script>

<template>
    <h5>Carousel</h5>
  
    <div id="carouselSlides" class="carousel slide" data-ride="carousel">
        <div class="carousel-inner">

            <div v-for="file in model.files.$values" :class="file.id === file1stId? 'carousel-item active': 'carousel-item'" :key="file.id">

                <img class="d-block w-100" :src="fileUrl + file.fileName">
            </div>

        </div>
        <a class="carousel-control-prev" href="#carouselSlides" role="button" data-slide="prev">
            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
            <span class="sr-only">Previous</span>
        </a>
        <a class="carousel-control-next" href="#carouselSlides" role="button" data-slide="next">
            <span class="carousel-control-next-icon" aria-hidden="true"></span>
            <span class="sr-only">Next</span>
        </a>
    </div>
</template>

