<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    import {default as AddAction} from './AddAction.vue'
    import { WorkflowAction } from '../models';
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
    import { BInputGroup, BFormInput, BFormTextarea, BFormSelect, BListGroup, BListGroupItem } from 'bootstrap-vue-3';
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const selectedButtons = ref([] as Guid[]);
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
    const setAccordion = (Id: Guid) => {
        if(selectedButtons.value.includes(Id)){
            const idx = selectedButtons.value.findIndex(sb => sb == Id)
            selectedButtons.value?.splice(idx as number, 1)
        }else{
            selectedButtons.value.push(Id);
        }
    }
</script>

<template>
    <div class="list-item">
        <b-list-group>
            <b-list-group-item v-for="action in store.workflow?.actions" :key="action.name"><span class="accordion-header">{{ action.name }}</span>
                <b-button v-b-toggle="action.name.replaceAll(' ', '')" class="accordion-button"  @click="setAccordion(action.id as Guid)">
                    <font-awesome-icon  icon="fa-chevron-down" class="fa-icon down-arrow" v-if="!selectedButtons.includes(action.id)" />
                    <font-awesome-icon  icon="fa-chevron-up" class="fa-icon up-arrow" v-if="selectedButtons.includes(action.id)" /></b-button>
                <b-collapse :id="action.name.replaceAll(' ', '')">
                    <b-card>
                        <b-card-text><b>Name :  {{action.name}}</b></b-card-text>
                        <b-card-text v-if="action.formTemplate != (Guid.EMPTY as unknown as Guid)"><b>Template :  {{action.formTemplate}}</b></b-card-text>
                        <b-card-text v-if="action.formView"><b>Form View :  {{action.formView}}</b></b-card-text>
                        <b-card-text v-if="action.buttons.length>0"><b>Buttons : <span v-for="button in action.buttons"><span class="one-space">{{button.label}}</span></span></b></b-card-text>
                        <b-card-text v-if="action.authorizations.length>0"><b>Authorization :  </b></b-card-text>
                    </b-card>
                </b-collapse>
                <span style="display:inline">
                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAction(action.id as Guid)"/>
                    <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editAction(action.id as Guid)" />
                </span>
            </b-list-group-item>
        </b-list-group>
    </div>
    <div class="header-style">Actions <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showActionPanel">
        <add-action  :editMode="editMode" :editActionId="(actionId as Guid)"/>
    </div>
</template>

