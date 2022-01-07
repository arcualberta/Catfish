<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { MultilingualTextField } from '../../models/fieldContainer'
    import TextCollection from './text/TextCollection.vue'

    export default defineComponent({
        name: "TextField",
        props: {
            model: {
                type: null as PropType<MultilingualTextField> | null,
                required: true
           },
            isMultivalued: 
            {
                type: Boolean,
                required: false,
                default: true
            },
            isMultiline: {
                type: Boolean,
                required: false,
                default: true
            }

        },
        components: {
            TextCollection
        }
    });
</script>

<template>
    <div>Text Field</div>
    <div>{{JSON.stringify(model)}}</div>
   <!--<div v-for="val in model.values" :key="val.id">
       <TextCollection :model="val" />
   </div>-->
</template>

