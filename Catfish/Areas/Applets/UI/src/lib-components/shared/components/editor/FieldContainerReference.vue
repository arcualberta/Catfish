<script lang="ts">
    import { defineComponent, PropType, ref, computed } from 'vue'
    import { useStore } from 'vuex';
	import { FieldContainerReference } from '../../models/fieldContainer'

    import ChildFieldContainer from './text/ChildFieldContainer.vue'

    export default defineComponent({
		name: "FieldContainerReference",
        components: {
           ChildFieldContainer
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
    <ChildFieldContainer :model="source" />
</template>

