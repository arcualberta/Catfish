  <script lang="ts">
    import { defineComponent, PropType } from 'vue'
      
    import * as models from '../../models'
	
    import SingleText from './SingleText.vue'

    export default defineComponent({
        name: "TextCollectionInput",
        components: {
            SingleText
        },
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
        },
       
        
    });
</script>

  <template>
      <div v-for="val in model?.values?.$values" :key="val.id">
          <SingleText :model="val" :is-multiline="isMultiline" />
      </div>
  </template>

