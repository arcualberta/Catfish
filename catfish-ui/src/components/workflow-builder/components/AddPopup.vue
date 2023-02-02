<script setup lang="ts">
    import { ref } from 'vue'
    import { eByttonReturnTypeValues, getButtonReturnTypeLabel } from "../../../components/shared/constants"
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store'
    import { WorkflowPopup, PopupButton } from '../models'
    import { QuillEditor } from '@vueup/vue-quill'
    import '@vueup/vue-quill/dist/vue-quill.snow.css'

    const store = useWorkflowBuilderStore();
    const props = defineProps < { editMode: boolean,
                                    editPopupId: Guid } > ();

    const popup = ref({} as unknown as WorkflowPopup);
    const button = ref({} as unknown as PopupButton);
    const buttons = ref([] as unknown as PopupButton[]);
    const addButtons = ref(false);
    
    if(props.editMode){
      
      const popupValues = store.workflow?.popups?.filter(p => p.id == props.editPopupId ) as WorkflowPopup[];
      popup.value.id = popupValues[0].id;
      popup.value.title = popupValues[0].title;
      popup.value.Message = popupValues[0].Message;
      popupValues[0].buttons!.forEach((btn) => {
          let newButton = {
          id : btn.id,
          text : btn.text ,
          returnValue : btn.returnValue
          }  as PopupButton
      buttons.value!.push(newButton);  
      })
    }else{
        popup.value.id = Guid.EMPTY as unknown as Guid;
        popup.value.title = "";
        popup.value.Message = "";
    }
    const toggleButtons = () => {
        addButtons.value = true;
    }
    const addPopup = (id : Guid)=>{
        if(id == Guid.EMPTY as unknown as Guid){
            let newPopup = {
                id : Guid.create(),
                title : popup.value.title,
                Message : popup.value.Message,
                buttons : buttons.value
            } as WorkflowPopup
            store.workflow?.popups?.push(newPopup);
        }else{
            store.workflow?.popups!.forEach((p)=> {
                if(p.id === id){
                    p.title = popup.value.title,
                    p.Message = popup.value.Message,
                    p.buttons = buttons.value
                }    
            })
        }
        store.showPopupPanel = false;
        buttons.value = [];
        resetPopup();
    }
    const addButton = (id : Guid)=>{
        if(id === Guid.EMPTY as unknown as Guid){
            let newButton = {
                id : Guid.create(),
                text : button.value.text,
                returnValue : button.value.returnValue 
            } as PopupButton
            buttons.value?.push(newButton);
            addButtons.value = false;
            resetButtons();
        }
    }
    const deleteButton = (id : Guid) => {
        const idx = buttons.value?.findIndex(btn => btn.id == id)
        buttons.value?.splice(idx as number, 1)
    }
    const resetPopup = () => {
        popup.value.id = Guid.EMPTY as unknown as Guid;
        popup.value.title = "";
        popup.value.Message = "";
        resetButtons();
    }
    const resetButtons = () => {
        button.value.text = "";
        button.value.returnValue = "";
        button.value.id = Guid.EMPTY as unknown as Guid;
    }
    const deletePanel = () => {
        store.showPopupPanel = false;
        buttons.value = [];
        resetPopup();
    }
</script>

<template>
    <div v-if="store.showPopupPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <div class="panel-delete">
                <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deletePanel()"/>
            </div>
            <b-input-group prepend="Title" class="mt-3">
                <b-form-input v-model="popup.title" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Message" class="mt-3">
                <QuillEditor v-model:content="popup.Message" contentType="html" theme="snow"  class="text-editor"></QuillEditor>
            </b-input-group>
            <br>
            <b-list-group class="col-sm-6">
                <b-list-group-item v-for="button in buttons" :key="(button.id .toString())">
                    <span>{{button.text}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteButton(button.id as Guid)"/>
                    </span>
                </b-list-group-item>
            </b-list-group>
            <div class="header-style">Buttons <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetButtons();toggleButtons()"/></div>
           <ConfirmPopUp v-if="addButtons" >
                <template v-slot:header>
                    Add a Button.
                    <button type="button" class="btn-close" @click="addButtons=false">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="Text" class="mt-3">
                        <b-form-input v-model="button.text" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Return Value" class="mt-3">
                        <select class="form-select" v-model="button.returnValue">
                            <option v-for="button in eByttonReturnTypeValues" :value="button">{{getButtonReturnTypeLabel(button)}}</option>
                        </select>
                    </b-input-group>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(button.id as Guid)">Add button</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addPopup(popup.id as Guid)">Add</button>
            </div>
        </div>
    </div>
  
</template>