<script lang="ts">
    import { defineComponent,computed, ref } from "vue";
    import { useStore } from 'vuex'
   
  
    export default defineComponent({
        name: "ItemList",
       
        props: {},
        setup() {
            const store = useStore()
            let items = ref(store.state.searchResult?.items)

            //let total = ref(store.state.searchResult?.count);
            return {
                items,
                total: computed(() => store.state.searchResult?.count),
               // items: computed(() => store.state.searchResult?.items),
                count: computed(() => store.state.searchResult?.count),
                first: computed(() => store.state.searchResult?.first),
                last: computed(() => store.state.searchResult?.last)
            }
        }
    });
</script>

<template>
    <div class="itemList">
        <h1>Result List</h1>
        <div>
            Item found: {{total}} </div>

            <div v-if="items?.length > 0">
                <div>
                    Item found: {{total}}
                    <!--<div v-for="item in items" :key="item.id">
                    {{item.title}}
                </div>-->
                </div>
            </div>
            <div v-else>No results found =(</div>
        </div>

</template>