<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    
    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'

    export default defineComponent({
  
        name: "SingleText",
        props: {
            model: {
                type: null as PropType<models.Text> | null,
                required: true
            },
            isMultiline: {
                type: null as PropType<boolean> | null,
                require: false,
                default: false
            },
            fieldType: {
                type: null as PropType<string> | null,
                require: false,
                default: "text"
            },
            numericStep: {
                type: null as PropType<number> | null,
                default: null
            }
			
        },
        setup(p) {

            const formStore = useFormSubmissionStore();

            const content = computed({
                get: () => p.model.value,
                set: (value) => formStore.setTextValue(p.model.id, value),
            })

            return {
                content
            }
        }
	
    });
</script>

<template>
     <div v-if="isMultiline === true"  >
        <textarea cols="30" rows="2" v-model="content"  />
     </div>
     <div v-else>
         <input v-if="numericStep" :step="numericStep" :type="fieldType" v-model="content" class="resized-textbox" />
         <input v-else :type="fieldType" v-model="content" class="resized-textbox" />
     </div>
</template>

