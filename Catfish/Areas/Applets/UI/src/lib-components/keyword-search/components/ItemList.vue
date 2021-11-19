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
        <div v-for="item in items" :key="item.id">
            <div class="item">
                <h2>  {{item.title}}</h2>
                <h3>  {{item.subtitle}}</h3>
                <div>  {{item.content}}</div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .item{
      margin: 10px 0;
    }
</style>