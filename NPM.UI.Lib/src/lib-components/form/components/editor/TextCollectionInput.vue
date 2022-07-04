  <script lang="ts">
    import { defineComponent, PropType } from 'vue'
      
    import * as models from '../../models'
	
      import TextInput from './TextInput.vue'

      export default defineComponent({
          name: "TextCollectionInput",
          components: {
              TextInput
          },
          props: {
              model: {
                  type: null as PropType<models.TextCollection> | null,
                  required: true
              },
              isMultiline: {
                  type: null as PropType<boolean> | null,
                  require: false,
                  default: false
              }
          },


      });
</script>

  <template>
      <div v-for="val in model?.values?.$values" :key="val.id">
          <TextInput :model="val" :is-multiline="isMultiline" />
      </div>
  </template>

