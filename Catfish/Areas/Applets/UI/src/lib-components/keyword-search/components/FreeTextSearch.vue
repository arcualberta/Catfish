<script lang="ts">
    import { defineComponent, computed, PropType} from 'vue'
    import { useStore } from 'vuex'

    import { state } from '../store/state'
    import {Actions, actions } from '../store/actions'
    import { /*Mutations,*/ mutations } from '../store/mutations'
    import { getters } from '../store/getters'
 

    export default defineComponent({
        name: "FreeTextSearch",
        components: {
         
        
        },
        props: {
            
        },
        setup() {
        
      
            const store = useStore()
            //const savedText = ref(store.state.freeTextSearch);
            //console.log("saved Text " + savedText);
            return {
               store,  
                freeTextSearch: computed(() => store.state.freeTextSearch),
              
            };
        },
        storeConfig: {
            state,
            actions,
            mutations,
            getters
        },
        methods: {
            onBlur: function (e: any) {
               
                if (e.target.value.length > 0) {
                    this.store.dispatch(Actions.SET_SEARCH_TEXT, e.target.value);
                }
            }
        }
       
    });
</script>

<template>
 
    <div class="input-group dir-text-search">
            <input type="text"  class="form-control rounded" placeholder="searchText" aria-label="Search" aria-describedby="search-addon"  @blur="onBlur($event)" />
           
            <button type="button" class="btn btn-outline-primary">search</button> 
            <span>{{freeTextSearch}} </span>
    </div>
       
</template>

