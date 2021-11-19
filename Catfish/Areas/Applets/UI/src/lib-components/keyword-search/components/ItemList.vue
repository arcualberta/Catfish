<script lang="ts">
    import { defineComponent} from "vue";
   
  
    export default defineComponent({
        name: "ItemList",
       
        props: {},
        setup() {

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