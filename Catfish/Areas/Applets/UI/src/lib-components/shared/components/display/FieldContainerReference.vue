<script lang="ts">
    import { Guid } from "guid-typescript";
    import { defineComponent, PropType, ref } from 'vue'
    import { useStore } from 'vuex';
    import { FieldContainerReference, EntityModelMethods, FieldContainer } from '../../models/fieldContainer'

   // import { Item } from '../../../item-viewer/models/item'
  
	import ChildFeildContainer from './ChildFeildContainer.vue'

    export default defineComponent({
        name: "FieldContainerReference",
        components: {
			ChildFeildContainer
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
            const source = ref(store.getters.metadataSet(p?.model?.refId));
            
           
             //const  mdSets = store.state.item.metadataSets; //source.metadataSets;
             //   console.log("metadatasets: " + JSON.stringify(mdSets));
           
             //  const dtContainer = source.dataContainer;

           
           /* console.log("datacontainer: " + JSON.stringify(dtContainer))*/
            return {
				refId,
				source
            }
        },
        methods: {

            getMetadaset(src: FieldContainer[], id: Guid) {
                return EntityModelMethods.getMetadataset(src, id);
            }
        }
       
    });
</script>

<template>
    Field Container Reference
    {{refId}}
    {{JSON.stringify(source)}}
    <ChildFeildContainer :model="source" />


</template>

