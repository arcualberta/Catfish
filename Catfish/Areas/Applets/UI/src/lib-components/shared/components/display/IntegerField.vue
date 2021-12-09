<script lang="ts">
    import { defineComponent, PropType} from 'vue'
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

        }
       
    });
</script>

<template>
    <div v-for="val in model.values">{{val.value}}</div>
</template>

