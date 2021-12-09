<script lang="ts">
    import { defineComponent, PropType, ref} from 'vue'
    import { MonolingualTextField } from '../../models/fieldContainer'


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
    <div>{{val.value}}</div>
</template>

