<script setup lang="ts">
    import { WorkflowState } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { ref, watch } from 'vue';
    import { Guid } from 'guid-typescript';

    let state = ref({} as unknown as WorkflowState)
    const store = useWorkflowBuilderStore();
    const addStates = ref(false);
    const editMode = ref(false);
    const newStateGuid = ref(Guid.create() as unknown as Guid);
    const ToggleAddStates = () => (addStates.value = !addStates.value);
    let disabled = ref(true);

    watch(() => state.value.name, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })
    const addState = (id : Guid)=>{
        if(id === Guid.EMPTY as unknown as Guid){
            let newState= {
                id: newStateGuid.value,
                name :state.value.name,
                description : state.value.description
            } as WorkflowState;
            store.workflow?.states?.push(newState);
        }else{
            store.workflow?.states!.forEach((st)=> {
                if(st.id === state.value.id){
                    st.name= state.value.name;
                    st.description= state.value.description;
                }    
             })
             editMode.value=false;
        }
        resetFields()
        addStates.value=false;
        
    }
    const resetFields = ()=>{
        state.value.id = Guid.EMPTY as unknown as Guid
        state.value.name = ""
        state.value.description = ""
    }
    const deleteState = (stateId : Guid) => {
        const idx = store.workflow?.states?.findIndex(opt => opt.id == stateId)
        store.workflow?.states?.splice(idx as number, 1)
    }
    const editState = (editStateId : Guid) => {
        const stateValues = store.workflow?.states?.filter(opt => opt.id == editStateId) as WorkflowState[]
        state.value.name = stateValues[0].name 
        state.value.description = stateValues[0].description as string
        state.value.id = stateValues[0].id
        editMode.value = true
        addStates.value = true
    }
</script>

<template>
    <div class="list-item">
        <b-list-group>
            <b-list-group-item v-for="state in store.workflow?.states" :key="state.name">
                <span>{{state.name}}</span>
                <span style="display:inline">
                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteState(state.id as Guid)"/>
                    <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editState(state.id as Guid)" />
                </span>
            </b-list-group-item>
        </b-list-group>
    </div>
     <div class="header-style">States <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetFields();ToggleAddStates()"/></div>

    <ConfirmPopUp v-if="addStates" >
        <template v-slot:header>
            Add a State.
            <button type="button" class="btn-close" @click="addStates=false">x</button>
        </template>
        <template v-slot:body>
            <div >
            <b-input-group prepend="Name" class="mt-3">
                <b-form-input v-model="state.name" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="(state.description as string)" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
        </div>
        </template>
        <template v-slot:footer>
            <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addState(state.id as Guid)">Add</button>
        </template>
    </ConfirmPopUp>
</template>
