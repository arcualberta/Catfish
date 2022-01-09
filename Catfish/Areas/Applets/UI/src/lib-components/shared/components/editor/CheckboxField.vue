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
			getSelectedFieldLabels(field: OptionsField) {
				return OptionsFieldMethods.getSelectedFieldLabels(field.options);
            }
		}
    });
</script>

<template>
    <div>Options Field</div>
    <div>{{JSON.stringify(model)}}</div>
    <!--{{getSelectedFieldLabels(model)}}-->
</template>
