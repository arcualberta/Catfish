<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'
    import Text from './text/Text.vue'
    import { validateMonolingualNumberField, /*RegExpressions*/ } from '../../store/form-validators'


    export default defineComponent({
        name: "IntegerField",
        props: {
            model: {
                type: null as PropType<MonolingualTextField> | null,
                required: true
           },
            isMultivalued: 
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
            const validationStatus = computed(() => validateMonolingualNumberField(p.model));
            const type = p.model.modelType;
            return {

                validationStatus,
                type

            }

        }
       
    });
</script>

<template>
    
    <div v-for="val in model?.values?.$values">
        <Text :model="val" :is-multiline="false" :is-rich-text="false" :validation-status="validationStatus" field="number" />
    </div>
</template>

