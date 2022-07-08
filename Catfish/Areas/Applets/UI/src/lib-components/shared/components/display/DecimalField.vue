<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'
    

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
        //setup(prop) {
        //    const val = ref(prop.model?.values?.slice(0, 1));
        //    let valNumber = Number(val.value);

        //    return {
        //        valNumber
        //    }
        //},
        methods: {
            formatToDecimal: (value : number, decimalPlaces : number) => {
                return Number(value).toFixed(decimalPlaces);
            }
           
        }
    });
</script>

<template>
    <div v-for="val in model.values.$values">{{formatToDecimal(val, numDecimalPlaces)}}</div>
</template>

