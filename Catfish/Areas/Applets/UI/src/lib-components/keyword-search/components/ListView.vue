<script lang="ts">
    import { defineComponent } from 'vue'
   
    import KeywordFilter from './KeywordFilter.vue'
   
   

    import ItemList from './ItemList.vue'

    export default defineComponent({
        name: "ListView",
        components: {
            KeywordFilter,
            ItemList
           
        }
       
    });
</script>

<template>   
   
    <div class="col-md-4 text-left">
            <KeywordFilter  />
     </div>
    <div class="col-md-8">
            <ItemList />
     </div>
  
</template>

