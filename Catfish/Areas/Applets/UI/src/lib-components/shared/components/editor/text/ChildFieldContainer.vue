<script lang="ts">
    import { defineComponent, PropType } from 'vue'

	import { FieldContainer } from '../../../models/fieldContainer'
	import Field from '../Field.vue'
   
    export default defineComponent({
        name: "ChildFieldContainer",
        props: {
            model: null as PropType<FieldContainer> | null,
        },
        components: {
			Field          
        }
    });
</script>

<template>
    <Field v-for="field in model.fields" :model="field" />
</template>