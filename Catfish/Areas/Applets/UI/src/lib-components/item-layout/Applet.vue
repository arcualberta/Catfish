<script lang="ts">
   // import { Guid } from 'guid-typescript'
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions/*, Actions*/ } from './store/actions'
    import { getters } from './store/getters'
    import { mutations/*, Mutations*/ } from './store/mutations'
    import props, { /*QueryParameter,*/ DataAttribute } from '../shared/props'

   


    export default defineComponent({
        name: "ItemLayout",
        components: {
          
        },
        props,
        setup(p) {
            const store = useStore();
            const dataAttributes = p.dataAttributes as DataAttribute;
            //console.log('Item Viewer setup ...');
            //console.log('props: ', JSON.stringify(p));
            const isAdmin = dataAttributes["is-admin"] as string;
            console.log('isAdmin: ', isAdmin);
            const selectedComponents = dataAttributes["selected-components"] as string;
           // const components = JSON.parse(selectedComponents);


           // const queryParams = p.queryParameters as QueryParameter;

           // store.commit(Mutations.SET_ID, components[0].formId);

            //load the data
           // store.dispatch(Actions.LOAD_ITEM);

            return {
                store,
                //queryParams,
                dataItem: computed(() => store.getters.rootDataItem),
                isAdmin,
              //  components,
                selectedComponents
            }
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters,

        },
        methods: {

          
        }
    });
</script>

<template>
   
    <div class="item">
        <h3>ItemLayout</h3>
        {{selectedComponents}}
      

        <!--<div>
            <h3>Item </h3>
            {{sate.item}}
        </div>-->
    </div>
</template>

