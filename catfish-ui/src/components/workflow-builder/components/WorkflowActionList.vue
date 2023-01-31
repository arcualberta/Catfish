<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    import {default as AddAction} from './AddAction.vue'
    import { WorkflowAction } from '../models';
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const visible = ref(false);
    const actionId = ref(Guid.EMPTY as unknown as Guid);
    const Toggle = () => (store.showActionPanel = true)
    const deleteAction = (id: Guid)=>{
        const idx = store.workflow?.actions.findIndex(ac => ac.id == id)
        store.workflow?.actions.splice(idx as number, 1)
    }
    const editAction = (Id: Guid) => {
        editMode.value = true;
        actionId.value = Id;
        store.showActionPanel = true;
    }
</script>

<template>
    <div class="list-item">
        <b-list-group>
            <b-list-group-item v-for="action in store.workflow?.actions" :key="action.name">{{ action.name }}
                <b-button v-b-toggle="action.name" class="accordion-button m1"  @click="visible = !visible">
                    <font-awesome-icon  icon="fa-chevron-down" class="fa-icon down-arrow" v-if="!visible" />
                    <font-awesome-icon  icon="fa-chevron-up" class="fa-icon up-arrow" v-if="visible" /></b-button>
                <b-collapse :id="action.name">
                    <b-card>{{action.name}}</b-card>
                </b-collapse>
                <span style="display:inline">
                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAction(action.id as Guid)"/>
                    <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editAction(action.id as Guid)" />
                </span>
            </b-list-group-item>
        </b-list-group>
    </div>
    <div class="accordion" role="tablist" v-for="action1 in store.workflow?.actions" :key="action1.name">
        
    </div>
    <div class="header-style">Actions <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showActionPanel">
        <add-action  :editMode="editMode" :editActionId="(actionId as Guid)"/>
    </div>
</template>

