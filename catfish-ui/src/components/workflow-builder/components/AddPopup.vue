<script setup lang="ts">
    import { ref, watch,computed } from 'vue';
    import { eButtonReturnType } from "../../../components/shared/constants";
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { WorkflowPopup, PopupButton } from '../models'

    const store = useWorkflowBuilderStore();
    const props = defineProps < { editMode: boolean,
                                    editTriggerId: string } > ();

    const popupId = ref("");
    const popupTitle = ref("");
    const popupMessage = ref("");
    const buttonId = ref("");
    const buttonText = ref("");
    const returnValue = ref("");
    const addButtons = ref(false);
    const popupButtons = ref(store.popupButtons)
    const returnTypes = computed(() => eButtonReturnType);

    const toggleButtons =()=>{
        addButtons.value= true;
    }
    const addPopup =(id:string)=>{
        if(id.length === 0){
            let newPopup= {
                id:Guid.create(),
                title:popupTitle.value,
                Message:popupMessage.value,
                buttons: store.popupButtons
            } as WorkflowPopup
            store.workflow?.popups?.push(newPopup);
            store.showPopupPanel = false;
            resetPopup();
        }
    }
    const addButton =(id:string)=>{
        if(id.length === 0){
            let newButton= {
                id:Guid.create(),
                text:buttonText.value,
                returnValue: returnValue.value 
            } as PopupButton
            store.popupButtons?.push(newButton);
            addButtons.value = false;
            resetButtons();
        }
    }
    const deleteButton =(id:Guid)=>{
        const idx = store.popupButtons?.findIndex(btn => btn.id == id)
        store.popupButtons?.splice(idx as number, 1)
    }
    const resetPopup =()=>{
        popupTitle.value="";
        popupMessage.value = "";
    }
    const resetButtons =()=>{
        buttonText.value="";
        returnValue.value = "";
    }
</script>

<template>
    <div v-if="store.showPopupPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <b-input-group prepend="Title" class="mt-3">
                <b-form-input v-model="popupTitle" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Message" class="mt-3">
                <b-form-textarea v-model="popupMessage" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
            <br>
            <b-list-group class="col-sm-6">
                <b-list-group-item v-for="button in store.popupButtons" :key="(button.id .toString())">
                    <span>{{button.text}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteButton(button.id as Guid)"/>
                    </span>
                </b-list-group-item>
            </b-list-group>
            <div class="header-style">Buttons <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetButtons();toggleButtons()"/></div>
           <ConfirmPopUp v-if="addButtons" >
                <template v-slot:header>
                    Add a Recipient.
                    <button type="button" class="btn-close" @click="addButtons=false">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="Text" class="mt-3">
                        <b-form-input v-model="buttonText" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Recipient Type" class="mt-3">
                        <b-form-select v-model="returnValue" :options="returnTypes"></b-form-select>
                    </b-input-group>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(buttonId)">Add recipient</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addPopup(popupId)">Add</button>
            </div>
        </div>
    </div>
  
</template>