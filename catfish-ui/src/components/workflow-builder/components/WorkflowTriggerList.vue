<script setup lang="ts">
    import { ref, watch } from 'vue';
    import {default as AddTrigger} from './AddTrigger.vue'
    import { useWorkflowBuilderStore } from '../store';

    const store = useWorkflowBuilderStore();

    const ToggleAddStates = () => {
        store.showAddTrigger = true;
    }
    
    const resetFields = () => {

}
</script>

<template>
    <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="trigger in store.workflow?.triggers" :key="trigger.name">
                    <span>{{trigger.name}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click=""/>
                        <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="" />
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
    <div class="header-style">Triggers <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="ToggleAddStates()"/></div>
    <div v-if="store.showAddTrigger">
        <AddTrigger />
    </div>
    
</template>