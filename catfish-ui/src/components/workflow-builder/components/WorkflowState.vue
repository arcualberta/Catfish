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
    const isEditState = ref(false);
    const stateName = ref("");
    const stateDescription = ref("");
   //  const requiredField = ref("hide");

    const disabled=computed(()=> stateName.value?.length > 0? false : true);
    let stateToEdit: WorkflowState=null; //temporary state before editing

   const addState = ()=>{
        let _name = stateName.value;
        let _description = stateDescription.value;
        //console.log("add new: " + isAddNewState.value + " edit: " + isEditState.value)
        if(isAddNewState.value){
            let newState= {
            id:Guid.create(),
            name :_name,
            description : _description
            } as WorkflowState;
            
            states.value?.push(newState);
             isAddNewState.value=false;
        }else{
            //edit existing one
             states.value?.forEach((st)=>{
               // console.log("curr state id: " + st.id + "prev state id " + stateToEdit.id)
                if(st.id === stateToEdit[0].id){
                    st.name= _name;
                    st.description= _description;
                }    
             })
             isEditState.value = false;
        }
      
       
       //reset the input fields
       stateName.value="";
       stateDescription.value="";
    }
   const removeState = (idx: number)=>{
        states.value?.splice(idx, 1);
   }
     const editState = (idx: number)=>{
        isEditState.value = true;
        isAddNewState.value=false;
        stateToEdit = states.value!.filter((st, index)=>{
            if(idx===index)
                return st as WorkflowState;
        });
       
        stateName.value = stateToEdit[0].name;
        stateDescription.value = stateToEdit[0].description as string;
       
   }

   const openPopUp = ()=>{
    if(stateName.value.length > 0)
       isEditState.value=true;
    else
        isAddNewState.value=true;
   }
</script>

<template>
     <h5>Workflow States</h5>
     <div v-if="states && states.length > 0">
      <b-row v-for="(state, idx) in states" :key="state.id">
            <b-col class="col-sm-4">
                <h6 >{{state.name}}</h6>
            </b-col>
            <b-col class="col-sm-6">
            <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" @click="editState(idx)" />
            <font-awesome-icon icon="fa-solid fa-circle-xmark" class="fa-icon" @click="removeState(idx)" />
       
            </b-col>
        </b-row>
        </div>
   
   <font-awesome-icon icon="fa-solid fa-circle-plus" @click="openPopUp()"/>Add New State

    <ConfirmPopUp v-if="isAddNewState || isEditState" >
                <template v-slot:header>
                    Add New State
                    <button type="button"
                            class="btn-close"
                            @click="isAddNewState=false; isEditState=false;">
                       
                    </button>
                </template>
                <template v-slot:body>
                    <b-row>
                        <div>Name : </div>
                        <div>
                            <input type="text" v-model="stateName" /> 
                          <!--  <span :class="requiredField">* Name is required </span>-->
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
                            @click="isAddNewState=false; isEditState=false;"  
                                 
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

<style scoped>
  .required{
    color: red;
  }
  .hide{
    display: none;
  }
</style>