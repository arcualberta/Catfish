<script lang="ts">
    import { defineComponent, /*onMounted,*/ ref } from 'vue'
    import { useStore } from 'vuex'

    import { state } from '../store/state'
    import {/* Actions,*/ actions } from '../store/actions'
    import { /*Mutations,*/ mutations } from '../store/mutations'
    import { getters } from '../store/getters'
   // import { SearchParams } from "./models"
   
   // import props, { DataAttribute } from '../shared/props'

    export default defineComponent({
        name: "FreeTextSearch",
        components: {
         
        
        },
       
        setup() {
           

          //  const dataAttributes = p.dataAttributes as DataAttribute;

            
          ////  const enableFreeTextSearch = dataAttributes["enable-freetext-search"] as string;
          //  //We need to use store in this setup method. so let's load it first.
            const store = useStore()

          //  //Storing the page and block IDs in the store
          //  store.commit(Mutations.SET_SOURCE, { pageId: p.pageId, blockId: p.blockId });

          //  //See if we can load a SearchParams object from local storage
            
          //  const searchParamsStr = localStorage.getItem(store.getters.searchParamStorageKey);
          //  let searchParams;
          //  if (searchParamsStr && searchParamsStr.length > 0
          //      && (searchParams = JSON.parse(searchParamsStr) as SearchParams)
          //      && searchParams.keywords) {

          //      //Restoring the store state from data reloaded from the state
          //      store.commit(Mutations.SET_KEYWORDS, searchParams.keywords);
          //      store.commit(Mutations.SET_OFFSET, searchParams.offset);
          //      store.commit(Mutations.SET_PAGE_SIZE, searchParams.max);
          //  }
          //  else {
          //      //Dispatch an action to loaf keywords
          //      store.dispatch(Actions.INIT_FILTER);
          //  }

          //  //When the component is mounted, execute a search query based on the current patameters in the store.state.
          //  onMounted(() => store.dispatch(Actions.FILTER_BY_KEYWORDS));

            const keywordQueryModel = ref(store.state.keywordQueryModel);
            const freeTextSearch = ref(store.state.freeTextSearch);
            return {
            
                keywordQueryModel,
                freeTextSearch
            };
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters
        }
       
    });
</script>

<template>
 
    <div class="input-group dir-text-search">
            <input type="search" class="form-control rounded" placeholder="Search" aria-label="Search" aria-describedby="search-addon"  />
            <button type="button" class="btn btn-outline-primary">search</button> <span>{{freeTextSearch}}</span>
    </div>
       
</template>

