<script lang="ts">
   // import { Guid } from 'guid-typescript'
    import { defineComponent, computed } from 'vue'
    import { useStore } from 'vuex';

    import { state } from './store/state'
    import { actions, Actions} from './store/actions'
    import { getters } from './store/getters'
    import { mutations, Mutations } from './store/mutations'
    import props, { QueryParameter, DataAttribute } from '../shared/props'
    import { FieldLayout} from "./models/fieldLayout"
   


    export default defineComponent({
        name: "ItemLayout",
        components: {
          
        },
        props,
        setup(p) {
            const store = useStore();
            const dataAttributes = p.dataAttributes as DataAttribute;
           // const queryParams = dataAttributes["query-parameter"] as string;
            const isAdmin = dataAttributes["is-admin"] as string;
            const templateId = dataAttributes["template-id"] as string;
            const selectedComponents = dataAttributes["selected-components"] as string;
            const components = JSON.parse(selectedComponents);
           // console.log(JSON.stringify(components));

            //get all the unique formIds
            let uniqueFormIds = [...new Set(components.map((com: FieldLayout) => com.formTemplateId))];

            console.log("selected Forms Ids" + JSON.stringify(uniqueFormIds));
            const queryParams = p.queryParameters as QueryParameter;

            store.commit(Mutations.SET_ID, queryParams.iid);

            store.commit(Mutations.SET_TEMPLATE_ID, templateId);
            store.commit(Mutations.SET_FORM_IDS, uniqueFormIds);

            //load the data
           store.dispatch(Actions.LOAD_ITEM);
           console.log("selected Forms" + JSON.stringify(state.item));
            return {
                store,
                queryParams,
                dataItem: computed(() => store.getters.rootDataItem),
                isAdmin,
              //  components,
                selectedComponents,
                items: computed(() => store.state.items)
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

      
        <div>
        <h3>Item </h3>
            id : {{store.state.id}}
        {{store.state.item}}
    </div>
    </div>
</template>

