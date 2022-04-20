<script lang="ts">
    import { defineComponent, PropType, ref, computed } from 'vue'
    import { useStore } from 'vuex';
	import { FieldContainerReference } from '../../models/fieldContainer'

    import FieldPrimitive from './FieldPrimitive.vue'

    export default defineComponent({
		name: "FieldContainerReference",
        components: {
            FieldPrimitive
        },
        props: {
            model: {
                type: null as PropType<FieldContainerReference> | null,
                required: true
            }

        },
        setup(p) {
            const store = useStore();
            const refId = ref(p.model.refId);
           
            return {
                refId,
                source: computed(() => store.getters.metadataSet(refId.value)),
            }
        }       
    });
</script>

<template>
    <FieldPrimitive v-for="field in source.fields?.$values" :model="field" />
</template>

