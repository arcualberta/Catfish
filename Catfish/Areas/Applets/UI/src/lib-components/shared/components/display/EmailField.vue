<script lang="ts">
    import { defineComponent, PropType, ref } from 'vue'
    import { MonolingualTextInput } from '../../models/fieldContainer'


    export default defineComponent({
        name: "EmailField",
        props: {
            model: {
                type: null as PropType<MonolingualTextInput> | null,
                required: true
           },
            isMultivalue: 
            {
                type: Boolean as PropType<boolean>,
                required: false,
                default: false
            }

        },
        methods: {

        },
        setup(props) {

            const val = ref(props.model?.values?.slice(0, 1));
            return {
                val
            }
        }
    });
</script>

<template>
   <div> Email Field</div>
    <div>{{val.value}}</div>
</template>

