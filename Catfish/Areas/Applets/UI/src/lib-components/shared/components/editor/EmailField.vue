<script lang="ts">
import { defineComponent, PropType , computed} from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'
    import Text from './text/Text.vue'
    import { validateMonolingualTextField } from '../../store/form-validators'

    export default defineComponent({
        name: "EmailField",
        props: {
            model: {
                type: null as PropType<MonolingualTextField> | null,
                required: true
           },
            isMultivalue: 
            {
                type: Boolean,
                required: false,
                default: false
            }

        },
        components: {
            Text
        },
   
        setup(p) {
            const validationStatus = computed(() => validateMonolingualTextField(p.model));
            const type = p.model.modelType;
            return {
              
                validationStatus,
                type
                
            }
            
        }
    });
</script>

<template>
    
    <!--<div>{{JSON.stringify(model)}}</div>-->
    
        <div v-for="val in model?.values?.$values">
            <Text :model="val" :is-multiline="false" :is-rich-text="false" :validation-status="validationStatus" field="email" />
            <div>Validation Status: {{validationStatus}}</div>
        </div>
   
</template>

