<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'
    import Text from './text/Text.vue'
    import { validateMonolingualNumberField /*, RegExpressions*/ } from '../../store/form-validators'

    export default defineComponent({
        name: "DecimalField",
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
            },
            numDecimalPlaces: {
                type: Number,
                required: false,
                default: 2
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
         
        },
        //methods: {
        //    formatToDecimal: (value : number, decimalPlaces : number) => {
        //        return Number(value).toFixed(decimalPlaces);
        //    }
           
        //}
    });
</script>

<template>
    <div v-for="val in model?.values?.$values">
        <Text :model="val" :is-multiline="false" :is-rich-text="false" :validation-status="validationStatus" field="decimal" />
    </div>
</template>

