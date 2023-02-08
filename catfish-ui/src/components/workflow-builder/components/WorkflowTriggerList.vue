<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import {default as AddTrigger} from './AddTrigger.vue'
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const triggerId = ref(Guid.EMPTY as unknown as Guid);
    const Toggle = () => {
        editMode.value = false;
        triggerId.value = Guid.EMPTY as unknown as Guid;
        store.showTriggerPanel = true;
    }
    const deleteTrigger = (triggerId : Guid) => {
        const idx =store.workflow?.triggers?.findIndex(tmp => tmp.id == triggerId)
        store.workflow?.triggers?.splice(idx as number, 1)
    }
    const editTrigger = (Id : Guid) => {
        editMode.value = true;
        triggerId.value = Id;
        store.showTriggerPanel = true;
    }
</script>

<template>
    <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="trigger in store.workflow?.triggers" :key="trigger.name">
                    <span>{{trigger.name}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteTrigger(trigger.id as Guid)"/>
                        <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editTrigger(trigger.id as Guid)" />
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
    <div class="header-style">Triggers <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showTriggerPanel">
        <AddTrigger :editMode="editMode" :editTriggerId="(triggerId as Guid)"/>
    </div>
    
</template>