<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { ref, computed, watch} from 'vue';
    import {default as AddAction} from './AddAction.vue'
    import { eAuthorizedBy} from "../../../components/shared/constants";
    import { Authorization, WorkflowAction } from '../models';
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
    import { BInputGroup, BFormInput, BFormTextarea, BFormSelect, BListGroup, BListGroupItem } from 'bootstrap-vue-3';
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const selectedButtons = ref([] as Guid[]);
    const authList = ref([] as string[]);
    
    const actionId = ref(Guid.EMPTY as unknown as Guid);
    const Toggle = () => {
        editMode.value = false;
        actionId.value = Guid.EMPTY as unknown as Guid;
        store.showActionPanel = true
    }
    const getRole = (roleId : Guid) => (store.workflow?.roles.filter(r => r.id == roleId)[0]?.name);
    const getFormTemplate = (formId : Guid) => (store.entityTemplate?.forms.filter(et => et.id == formId)[0]?.name);
    const deleteAction = (id : Guid) => {
        const idx = store.workflow?.actions.findIndex(ac => ac.id == id)
        store.workflow?.actions.splice(idx as number, 1)
    }
    const editAction = (Id : Guid) => {
        editMode.value = true;
        actionId.value = Id;
        store.showActionPanel = true;
    }

    const checkAuth = (stateId:Guid, actionId : Guid): boolean => {
        console.log("Calling checkAuth")
        const auth = store.workflow?.actions.filter(a => a.id == actionId)[0].authorizations;

        const result = auth?.filter( au => au.currentStateId == stateId)?.length
        return result && result > 0 ? true : false;
    }
        
    const setAccordion = (Id : Guid) => {
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
                        <b-card-text v-if="action.formTemplateId != (Guid.EMPTY as unknown as Guid)"><b>Template :  {{getFormTemplate(action.formTemplateId as Guid)}}</b></b-card-text>
                        <b-card-text v-if="action.formView"><b>Form View :  {{action.formView}}</b></b-card-text>
                        <b-card-text v-if="action.buttons.length>0"><b>Buttons : <span v-for="button in action.buttons"><span class="one-space">{{button.label}}</span></span></b></b-card-text>
                        <b-card-text v-if="action.authorizations.length>0"><b>Authorization    </b>
                            <div v-for="state in store.workflow?.states">
                                
                                <div class="left-space" v-if="checkAuth(state.id as Guid, action.id as Guid)"><b> {{ state.name }} : </b>
                                    <span v-for="auth in action.authorizations">
                                        <span v-if="auth.currentStateId == state.id">
                                            <span v-if="auth.authorizedRoleId" class="one-space"><b>{{getRole(auth.authorizedRoleId as Guid)}}</b></span>
                                            <span v-if="auth.authorizedDomain" class="one-space"><b>{{auth.authorizedDomain}}</b></span>
                                            <span v-if="auth.authorizedBy==eAuthorizedBy.Owner" class="one-space"><b>Owner</b></span>
                                        </span>
                                    </span>
                                </div>
                            </div>
                        </b-card-text>
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

