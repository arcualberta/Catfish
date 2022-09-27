<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted, ref} from 'vue'
    import { useEntityEditorStore } from './store';
    import {default as EntitySummaryEditor} from './components/entity-summary-editor.vue'
    

    const props = defineProps<{
       // dataAttributes?: AppletAttribute | null,
        //queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia
    }>();

    const store = useEntityEditorStore(props.piniaInstance);
    const entityTemplate = computed(() => store.entityTemplate);
    

    onMounted(() => {
        store.loadTemplates();
       
    });

    const templateEntries = computed(()=>store.templates);
    let isShowEditor= ref(false);
    const createEntity = ()=>{
        store.createNewEntity();
    };

    const entity = computed(()=>store.entity)
</script>

<template>
    <h3>Entity Editor</h3>
    <div class="control">
        <button @click="createEntity()">New Entity</button>
        <button class="btn btn-success">Save</button>
    </div>
    <div v-if="entityTemplate" class="row mt-2 pt-2 border-top">
        <div class="col-sm-10"><button>Summary</button></div>
    </div>
    <!--<div v-if="selectedButton === 'summery'">-->
        <EntitySummaryEditor v-if="entity !== null" />
    <!--</div>-->
    <!--<div v-if="selectedButton === 'data'">
        Data
    </div>-->
    <div v-if="store.entityTemplate" class="alert alert-info mt-4"><h3>Entity Template</h3>{{store.entityTemplate}}</div>
    <div v-if="entity" class="alert alert-info mt-4"><h3>Entity</h3>{{entity}}</div>
</template>


<style scoped src="./style.css"></style>
