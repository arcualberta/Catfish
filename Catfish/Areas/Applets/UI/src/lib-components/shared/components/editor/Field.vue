<script lang="ts">
	import { defineComponent, PropType } from 'vue'
	import { Field, eFieldType } from '../../models/fieldContainer'
    import { FieldContainerUtils } from '../../store/form-submission-utils'

    import FieldPrimitive from './FieldPrimitive.vue'
    import FieldContainerReference from './FieldContainerReference.vue'

    export default defineComponent({
        name: "Field",
        props: {
            model: {
                type: null as PropType<Field> | null,
                required: true
			},
        },
		components: {
            FieldPrimitive,
            FieldContainerReference
		},
        setup(p) {

            return {
                isFieldContainerReference: FieldContainerUtils.getFieldType(p.model) == eFieldType.FieldContainerReference
            }
        }
    });
</script>

<template>
	<FieldContainerReference v-if="isFieldContainerReference" :model="model" />
	<FieldPrimitive v-else :model="model" />
</template>

