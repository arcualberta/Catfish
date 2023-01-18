<script setup lang="ts">
    import { WorkflowState } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {storeToRefs} from 'pinia';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { computed, ref, watch } from 'vue';
    import { Guid } from 'guid-typescript';

    const props = defineProps < { model: WorkflowState } > ();
    const store = useWorkflowBuilderStore();
    const stateName = props.model.name;
    const stateDescription = props.model.description;
    const addStates = ref(false);
    const editMode = ref(false);
    const ToggleAddStates = () => (addStates.value = !addStates.value);
    let disabled = ref(true);

    watch(() => props.model.name, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })
    const addState = ()=>{
        if(editMode.value===false){
            let newState= {
                id:Guid.create(),
                name :props.model.name,
                description : props.model.description
            } as WorkflowState;
        
            store.states?.push(newState);
        }else{
            const idx = store.states?.findIndex(opt => opt.id == props.model.id)
            store.states!.forEach((st)=> {
                if(st.id === props.model.id){
                    st.name= props.model.name;
                    st.description= props.model.description;
                }    
             })
             editMode.value=false;
        }
        
        addStates.value=false;
        props.model.name = ""
        props.model.description = ""
    }
    const deleteState = (stateId: Guid) => {
        const idx = store.states?.findIndex(opt => opt.id == stateId)
        store.states?.splice(idx as number, 1)
    }
    const editState = (stateId: Guid) => {
        const stateValues = store.states?.filter(opt => opt.id == stateId) as unknown as WorkflowState
        console.log('stateValues', stateValues)
        props.model.name=stateValues[0].name
        props.model.description = stateValues[0].description
        props.model.id = stateValues[0].id
        editMode.value = true
        addStates.value = true
        
    }
</script>

<template>
    {{ store.states }}
    <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="state in store.states" :key="state.name">
                    <span>{{state.name}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteState(state.id as Guid)"/>
                        <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #2BB892; float: right;" @click="editState(state.id as Guid)" />
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
     <div class="header-style">States <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="ToggleAddStates()"/></div>

    <ConfirmPopUp v-if="addStates" >
                <template v-slot:header>
                    Add a State.
                    <button type="button" class="btn-close" @click="addStates=false">x</button>
                </template>
                <template v-slot:body>
                    <div >
                    <b-input-group prepend="Name" class="mt-3">
                        <b-form-input v-model="model.name" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Description" class="mt-3">
                        <b-form-textarea v-model="(model.description as string)" rows="3" max-rows="6"></b-form-textarea>
                    </b-input-group>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addState">Add</button>
                </template>
            </ConfirmPopUp>
</template>
