<script lang="ts">
    //import { Guid } from "guid-typescript";
    import { defineComponent, PropType, ref } from 'vue'
    import { useStore } from 'vuex';
    import { FieldContainerReference } from '../../models/fieldContainer'

   // import { Item } from '../../../item-viewer/models/item'
  
    import ChildFieldContainer from './ChildFieldContainer.vue'

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
            const source = ref(store.getters.metadataSet(refId));
            
            console.log("refId: " + JSON.stringify(refId))
            console.log("source :" + JSON.stringify(source))
            return {
				refId,
				source
            }
        },
        methods: {

            //getMetadaset(src: FieldContainer[], id: Guid) {
            //    return EntityModelMethods.getMetadataset(src, id);
            //}
        }
       
    });
</script>

<template>
    Field Container Reference
    {{refId}}
    {{JSON.stringify(source)}}
    <!--<ChildFieldContainer :model="source" />-->


</template>

