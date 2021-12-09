<script lang="ts">
    import { Guid } from "guid-typescript";
    import { defineComponent, PropType, ref } from 'vue'
    import { useStore } from 'vuex';
    import { FieldContainerReference, EntityModelMethods, FieldContainer } from '../../models/fieldContainer'

   // import { Item } from '../../../item-viewer/models/item'
  
    import FieldContainerVue from './FieldContainer.vue'

    export default defineComponent({
        name: "FieldContainerReference",
        components: {
            FieldContainerVue
        },
        props: {
            model: {
                type: null as PropType<FieldContainerReference> | null,
                required: true
            }

        },
        setup() {
            const store = useStore();
            const source = ref(store.getters.metadataSets);
            
           
             //const  mdSets = store.state.item.metadataSets; //source.metadataSets;
             //   console.log("metadatasets: " + JSON.stringify(mdSets));
           
             //  const dtContainer = source.dataContainer;

           console.log("item: " + JSON.stringify(source));
           
           /* console.log("datacontainer: " + JSON.stringify(dtContainer))*/
            return { source}
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
   <!--{{JSON.stringify(source)}}-->
    <FieldContainerVue :model="getMetadaset(source, model.id)" />
</template>

