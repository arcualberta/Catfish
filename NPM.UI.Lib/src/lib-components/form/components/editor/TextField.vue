<script lang="ts">
    import { defineComponent, PropType} from "vue";
    import * as models from '../../models'
    import MultivalueText from "./MultivalueText.vue"

    export default defineComponent({
        name: "TextField",
        components:{
           MultivalueText
        },
        props: {
            model: {
                type: null as PropType<models.TextField> | null,
                required: false
            },
        },
        
    });
</script>


<template>
   
    <div v-for="val in model.values.$values" :key="val.id"> 
       
        <MultivalueText :model="val" :isMultiline="false" />
    </div>
   
</template>
