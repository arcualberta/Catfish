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
        <div v-if="items?.length > 0">
            <div v-for="item in items">
                <div>
                    <div v-for="item in items" :key="item.id">
                        <div class="item">
                            <div class="itm-heading col-md-12">  {{item.title}}</div>
                            <div class="itm-subHeading col-md-12">  {{item.subtitle}}</div>
                            <div class="itm-content col-md-12">  {{item.content}}</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .item{
      margin: 10px 0;
    }
</style>