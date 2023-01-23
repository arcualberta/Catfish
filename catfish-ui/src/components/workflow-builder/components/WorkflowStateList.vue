<script setup lang="ts">
    import { WorkflowState } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { ref, watch } from 'vue';
    import { Guid } from 'guid-typescript';

    let stateId = ref("")
    let stateName = ref("")
    let stateDescription = ref("")
    const store = useWorkflowBuilderStore();
    const addStates = ref(false);
    const editMode = ref(false);
    const ToggleAddStates = () => (addStates.value = !addStates.value);
    let disabled = ref(true);

    const states = ref(store.workflow?.states);

    watch(() => stateName.value, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })
    const addState = (id:string)=>{
        if(id.length === 0){
            let newState= {
                id:Guid.create(),
                name :stateName.value,
                description : stateDescription.value
            } as WorkflowState;
        
            states.value?.push(newState);
        }else{
            
            const idx = states.value?.findIndex(opt => opt.id.toString() == stateId.value)
            states.value!.forEach((st)=> {
                if(st.id.toString() === stateId.value){
                    st.name= stateName.value;
                    st.description= stateDescription.value;
                }    
             })
             editMode.value=false;
        }
        resetFields()
        addStates.value=false;
        
    }
    const resetFields = ()=>{
        stateId.value = ""
        stateName.value = ""
        stateDescription.value = ""
    }
    const deleteState = (stateId: Guid) => {
        const idx = states.value?.findIndex(opt => opt.id == stateId)
        states.value?.splice(idx as number, 1)
    }
    const editState = (editStateId: Guid) => {
        console.log("stateId",stateId)
        const stateValues = states.value?.filter(opt => opt.id == editStateId) as WorkflowState[]
        stateName.value=stateValues[0].name 
        stateDescription.value = stateValues[0].description as string
        stateId.value = stateValues[0].id.toString()
        editMode.value = true
        addStates.value = true
    }
</script>

<template>
    <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="state in states" :key="state.name">
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
                <b-form-input v-model="stateName" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="stateDescription" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
        </div>
        </template>
        <template v-slot:footer>
            <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addState(stateId)">Add</button>
        </template>
    </ConfirmPopUp>
</template>
