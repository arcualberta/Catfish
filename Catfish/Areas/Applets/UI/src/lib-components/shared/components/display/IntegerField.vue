<script lang="ts">
    import { boolean } from 'joi';
    import { defineComponent, PropType, ref} from 'vue'
    import { MonolingualTextInput } from '../../models/fieldContainer'


    export default defineComponent({
        name: "IntegerField",
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
            }

        },
        setup(props) {

            const val = ref(props.model?.values?.slice(0, 1));

          

            return {
                val
            }
        },
        methods: {

        }
    });
</script>

<template>
   <div> Integer Field</div>
    <div>{{val.value}}</div>
</template>

