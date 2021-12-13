<script lang="ts">
import { defineComponent, PropType } from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'


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
    //    methods: {

    //    },
    //    setup(props) {

    //        const val = ref(props.model?.values?.slice(0, 1));
    //        return {
    //            val
    //        }
    //    }
    });
</script>

<template>
    <div v-for="val in model.values">{{val.value}}</div>
</template>

