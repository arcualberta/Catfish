<script lang="ts">
    import { defineComponent, PropType } from 'vue'
   
	import { OptionsField, OptionsFieldMethods } from '../../models/fieldContainer'
   

    export default defineComponent({
		name: "OptionsField",
        components: {
            
        },
        props: {
            model: {
                type: Object as PropType<OptionsField> | null,
                required: true
           }
        },
        methods: {
			getSelectedFieldLabels(field: OptionsField) {
				return OptionsFieldMethods.getSelectedFieldLabels(field.options);
            }
		}
    });
</script>

<template>
    {{getSelectedFieldLabels(model)}}
</template>
