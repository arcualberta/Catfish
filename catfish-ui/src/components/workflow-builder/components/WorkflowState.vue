<script setup lang="ts">
    import { WorkflowState } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {storeToRefs} from 'pinia';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
import { computed, ref } from 'vue';
import { Guid } from 'guid-typescript';


   // const props = defineProps < { stateName: string,
   //               stateDescription: string | null} > ();
    const store = useWorkflowBuilderStore();

    const {states}= storeToRefs(store);
    

    const isAddNewState = ref(false);
    const stateName = ref("");
    const stateDescription = ref("");

    const disabled=computed(()=>stateName.value.length > 0? false : true);
    const addState = ()=>{
    
        let _name = stateName.value;
        let _description = stateDescription.value;
        let newState= {
         id:Guid.create(),
           name :_name,
           description : _description
        } as WorkflowState;
        
        states.value?.push(newState);
        isAddNewState.value=false;
       
       
       stateName.value="";
       stateDescription.value="";
    
    }
   
</script>

<template>
     <div class="header-style">Workflow States</div>
     <ul v-if="states && states.length > 0">
        <li v-for="state in states" :key="state.id">
        <span>{{state.name}}</span>
        <span>
            <font-awesome-icon icon="fa-solid fa-pen-circle" />
            <font-awesome-icon icon="fa-solid fa-circle-xmark" />
        </span>
        </li>
   </ul>
   <font-awesome-icon icon="fa-solid fa-circle-plus" @click="isAddNewState = !isAddNewSate"/>Add New State

    <ConfirmPopUp v-if="isAddNewState" >
                <template v-slot:header>
                    Add New State
                    <button type="button"
                            class="btn-close"
                            @click="isAddNewState=false">
                       
                    </button>
                </template>
                <template v-slot:body>
                    <b-row>
                        <div>Name : </div>
                        <div>
                            <input type="text" v-model="stateName" /> 
                           
                        </div>
                    </b-row>
                    <b-row>
                        <div>Description : </div>
                        <div><input type="text" v-model="stateDescription" /></div>
                    </b-row>
                </template>
                <template v-slot:footer>
                    <button type="button"
                            class="modal-cancel-btn" 
                            @click="isAddNewState=false"  
                                 
                            aria-label="Close modal">
                        Cancel
                    </button>
                    
                    <button type="button"
                            class="modal-confirm-btn"
                            style="margin-left:10px"
                           @click="addState()"
                           :disabled="disabled" 
                            aria-label="Close modal">
                        Save
                    </button>
                    <div>Save button disabled: {{disabled}} </div>
                </template>
            </ConfirmPopUp>
</template>
<style>
.header-style{
    font-size: 24px;
    font-family: "Architects Daughter";
}</style>