<script setup lang="ts">
    //import { WorkflowState } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {storeToRefs} from 'pinia';
    import {default as ConfirmedPopUp} from "../../shared/components"
import { ref } from 'vue';

    const store = useWorkflowBuilderStore();

    const {states}= storeToRefs(store);

    const isAddNewState = ref(false);

</script>

<template>
     <h5>Workflow States</h5>
     <ul v-if="states && states.length > 0">
        <li v-for="state in states" :key="state.id">
        <span>state.name</span>
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
                        x
                    </button>
                </template>
                <template v-slot:body>
                    <b-row>
                        <div>Name : </div>
                        <div><input type="text" /></div>
                    </b-row>
                    <b-row>
                        <div>Description : </div>
                        <div><input type="text" /></div>
                    </b-row>
                </template>
                <template v-slot:footer>
                    <button type="button"
                            class="modal-cancel-btn"         
                            aria-label="Close modal">
                        Cancel
                    </button>
                    
                    <button type="button"
                            class="modal-confirm-btn"
                            style="margin-left:10px"
                           
                            aria-label="Close modal">
                        Save
                    </button>
                </template>
            </ConfirmPopUp>
</template>