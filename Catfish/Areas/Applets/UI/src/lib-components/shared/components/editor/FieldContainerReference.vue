<script lang="ts">
    import { defineComponent, PropType, ref, computed } from 'vue'
    import { useStore } from 'vuex';
	import { FieldContainerReference } from '../../models/fieldContainer'

    import FieldBase from './FieldBase.vue'

    export default defineComponent({
		name: "FieldContainerReference",
        components: {
            FieldBase
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
    <!--TODO: Implement editor template for FieldContainerReference-->
    <FieldBase v-for="field in source.fields?.$values" :model="field" />
</template>

