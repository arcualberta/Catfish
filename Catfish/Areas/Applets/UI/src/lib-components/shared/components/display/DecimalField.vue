<script lang="ts">
    import { boolean, number } from 'joi';
    import { defineComponent, PropType, ref} from 'vue'
    import { MonolingualTextInput } from '../../models/fieldContainer'
    

    export default defineComponent({
        name: "DecimalField",
        props: {
            model: {
                type: null as PropType<MonolingualTextInput> | null,
                required: true
           },
            isMultivalued: 
            {
                type: boolean,
                required: false,
                default: false
            },
            numDecimalPlaces: {
                type: number,
                required: true  
            }

        },
        setup(prop) {
            const val = ref(prop.model?.values?.slice(0, 1));
            let valNumber = Number(val.value);

            return {
                valNumber
            }
        },
        methods: {
            formatToDEcimal: (value : number, decimalPlaces : number) => {
                return Number(value).toFixed(decimalPlaces);
            }
           
        }
    });
</script>

<template>
   <div> Decimal Field</div>
    <div>{{formatToDecimal(valNumber, numDecimalPlaces)}}</div>
</template>

