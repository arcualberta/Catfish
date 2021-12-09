<script lang="ts">
    import { defineComponent, PropType } from 'vue'
   
	import { OptionsField, OptionsFieldMethods } from '../../models/fieldContainer'
   

    export default defineComponent({
        name: "CheckboxField",
        components: {
            
        },
        props: {
            model: {
                type: Object as PropType<OptionsField> | null,
                required: true
           }
        },
        methods: {
			concatenatedString(field: OptionsField) {
				return OptionsFieldMethods.getSelectedFieldLabels(field.options);
            }
		}
    });
</script>

<template>
    {{concatenatedString(model)}}
</template>

<style scoped>
    .checkboxField{
        margin-right:15px ;
    }
</style>