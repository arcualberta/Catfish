<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    import {default as AddAction} from './AddAction.vue'
    import { WorkflowAction } from '../models';
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
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
            <b-list-group-item v-for="action in store.workflow?.actions" :key="action.name">
                <span>{{action.name}}</span>
                <span style="display:inline">
                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAction(action.id as Guid)"/>
                    <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editAction(action.id as Guid)" />
                </span>
            </b-list-group-item>
        </b-list-group>
    </div>
    <div class="accordion" role="tablist" v-for="action1 in store.workflow?.actions" :key="action1.name">
        <b-card no-body class="mb-1">
      <b-card-header header-tag="header" class="p-1" role="tab">
        <b-button block v-b-toggle.{accordion-1} variant="info">{{action1.name}}</b-button>
      </b-card-header>
      <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
        <b-card-body>
          <b-card-text>I start opened because <code>visible</code> is <code>true</code></b-card-text>
          <b-card-text></b-card-text>
        </b-card-body>
      </b-collapse>
    </b-card>
    </div>
    <div class="header-style">Actions <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showActionPanel">
        <add-action  :editMode="editMode" :editActionId="(actionId as Guid)"/>
    </div>
</template>

