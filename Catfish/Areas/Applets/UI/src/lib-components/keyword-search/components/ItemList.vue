<script lang="ts">
    import { defineComponent, computed } from "vue";
    import { useStore } from 'vuex'
   
  
    export default defineComponent({
        name: "ItemList",
       
        props: {},
        setup() {
            const store = useStore()

            return {
                items: computed(() => store.state.searchResult?.items)
            }
        }
    });
</script>

<template>
    <div class="itemList">
        <h1>Result List</h1>
        <div>
            <div v-for="item in items">
                <div>
                    <div v-for="item in items" :key="item.id">
                        {{item.title}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>