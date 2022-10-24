<template>
    <div class="row mt-2">
            <div class="col-sm-6">
                <label>Search Target:</label>
                <select  v-model="selectedSearchTarget" class="form-select">
                    <option v-for="type in eSearchTarget" :key="type" :value="type">{{type}}</option>
                </select>
               
            </div>
            <div class="col-sm-6">
                <label> Search Text </label>
                <input type="text" v-model="searchText" class="form-control" @blur="performSearch" @keydown.enter="performSearch"/>
               
            </div>
        </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { eEntityType, eSearchTarget } from "../../constants";
 import { useEntitySelectStore } from './store';

 const props = defineProps<{
        storeId: string,
        entityType: eEntityType,      
    }>();
 const selectedSearchTarget=ref(eSearchTarget);
 const searchText= ref("")

const entityListStore = useEntitySelectStore(props.storeId);
const performSearch=()=> entityListStore.seach(props.entityType, selectedSearchTarget.value, searchText.value);
</script>
