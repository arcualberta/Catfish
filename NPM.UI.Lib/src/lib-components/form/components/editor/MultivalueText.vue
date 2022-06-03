  <script lang="ts">
    import { defineComponent, PropType } from 'vue'
   
    import * as models from '../../models'
	
    import SingleText from './SingleText.vue'

    export default defineComponent({
        name: "MultivalueText",
        props: {
            model: {
                type: null as PropType<models.TextCollection> | null,
                required: true
			},
            isMultiline:{
                type:  null as PropType<boolean> | null,
                require: false,
                default: false
            }
			//validationStatus: {
			//	type: null as PropType<eValidationStatus> | null,
			//	required: true
           // }
        },
        components: {
            SingleText
        }
    });
</script>

<template>
    <div v-for="val in model?.values?.$values" :key="val.id">
      
        <SingleText :model="val"  :isMultiline="isMultiline" />
    </div>

</template>

