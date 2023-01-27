<script setup lang="ts">
    import { ref,computed } from 'vue'
    import { eButtonReturnType,eByttonReturnTypeValues, getButtonReturnTypeLabel } from "../../../components/shared/constants"
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store'
    import { WorkflowPopup, PopupButton } from '../models'
    import { QuillEditor } from '@vueup/vue-quill'
    import '@vueup/vue-quill/dist/vue-quill.snow.css'

    const store = useWorkflowBuilderStore();
    const props = defineProps < { editMode: boolean,
                                    editPopuoId: string } > ();

    const popupId = ref("");
    const popupTitle = ref("");
    const popupMessage = ref("");
    const buttonId = ref("");
    const buttonText = ref("");
    const returnValue = ref("");
    const addButtons = ref(false);
    const returnTypes = computed(() => eButtonReturnType);
    
    if(props.editMode){
      
      const popupValues = store.workflow?.popups?.filter(p => p.id.toString() == props.editPopuoId ) as WorkflowPopup[];
      popupId.value=popupValues[0].id.toString();
      popupTitle.value=popupValues[0].title;
      popupMessage.value =popupValues[0].Message;
      popupValues[0].buttons!.forEach((btn)=> {
          let newButton={
          id:btn.id,
          text: btn.text ,
          returnValue: btn.returnValue
          }  as PopupButton
      store.popupButtons!.push(newButton);  
      })
  }
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
        }else{
            store.workflow?.popups!.forEach((p)=> {
                if(p.id.toString() === id){
                    p.title = popupTitle.value,
                    p.Message = popupMessage.value,
                    p.buttons = store.popupButtons as PopupButton[]
                }    
            })
        }
        store.showPopupPanel = false;
        store.popupButtons=[];
        resetPopup();
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
        resetButtons();
    }
    const resetButtons =()=>{
        buttonText.value="";
        returnValue.value = "";
    }
    const deletePanel =()=>{
        store.showPopupPanel = false;
        store.popupButtons=[];
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
                <b-form-input v-model="popupTitle" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Message" class="mt-3">
                <QuillEditor v-model:content="popupMessage" contentType="html" theme="snow"  class="text-editor"></QuillEditor>
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
                    Add a Button.
                    <button type="button" class="btn-close" @click="addButtons=false">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="Text" class="mt-3">
                        <b-form-input v-model="buttonText" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Return Value" class="mt-3">
                        <select class="form-select" v-model="returnValue">
                            <option v-for="button in eByttonReturnTypeValues" :value="button">{{getButtonReturnTypeLabel(button)}}</option>
                        </select>
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