<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    
  
   
   export default defineComponent({
            name: "PdfViewer",
       components: {
          
        },
       props: {
           fileUrl: {
               type: null as PropType<string> | null,
               required: false
           },
       },
       setup(p) {
           const pdfViewerPath = window.location.origin + "/assets/js/vendor/pdfjs-2.13.216/web/viewer.html?file=";
           return {
               fileUrl: computed(() => p.fileUrl),
               pdfViewerPath
           }
          
       }
       
    });
</script>

<template>

   
    <div class="pdfViewer">
      
        <iframe height="100%" width="100%" :src="pdfViewerPath + fileUrl"></iframe>
    </div>
</template>

<style scoped>
    .pdfViewer {
        width: 50%;
        height: 79vh;
        min-width: 400px;
    }
</style>