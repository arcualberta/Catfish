<script setup lang="ts">
    import { EmailTemplate } from '../models'
    import { useWorkflowBuilderStore } from '../store';
    import {storeToRefs} from 'pinia';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { computed, ref } from 'vue';
    import { Guid } from 'guid-typescript';

    const store = useWorkflowBuilderStore();

    const {emailTemplates}= storeToRefs(store);
    

    const isAddNewTemplate = ref(false);
    const isEditTemplate = ref(false);
    const templateName = ref("");
    const templateDescription = ref("");
    const templateSubject = ref("");
    const templateBody = ref("");
   //  const requiredField = ref("hide");

    const disabled=computed(()=> templateName.value?.length > 0? false : true);
    let templateToEdit: EmailTemplate=null; //temporary state before editing

   const addTemplate = ()=>{
        let _name = templateName.value;
        let _description = templateDescription.value;
        let _subject = templateSubject.value;
        let _body = templateBody.value;

        if(isAddNewTemplate.value){
            let newTemplate= {
            id:Guid.create(),
            name :_name,
            description : _description,
            emailSubject: _subject,
            emailBody: _body
            } as EmailTemplate;
            
            emailTemplates.value?.push(newTemplate);
             isAddNewTemplate.value=false;
        }else{
            //edit existing one
             emailTemplates.value?.forEach((et)=>{
               // console.log("curr state id: " + st.id + "prev state id " + stateToEdit.id)
                if(et.id === templateToEdit[0].id){
                    et.name= _name;
                    et.description= _description;
                    et.emailSubject=_subject;
                    et.emailBody=_body;
                }    
             })
             isEditTemplate.value = false;
        }
      
       
       //reset the input fields
       
       resetFields();
    }
   const removeTemplate = (idx: number)=>{
        emailTemplates.value?.splice(idx, 1);
   }
     const editTemplate = (idx: number)=>{
        isEditTemplate.value = true;
        isAddNewTemplate.value=false;
        templateToEdit = emailTemplates.value!.filter((et, index)=>{
            if(idx===index)
                return et as EmailTemplate;
        });
       
        templateName.value = templateToEdit[0].name;
        templateDescription.value = templateToEdit[0].description as string;
        templateSubject.value = templateToEdit[0].emailSubject;
        templateBody.value = templateToEdit[0].emailBody as string;
       
   }

   const openPopUp = ()=>{
    if(templateName.value.length > 0)
       isEditTemplate.value=true;
    else
        isAddNewTemplate.value=true;
   }

   const resetFields=()=>{
   // console.log("reset fields")
     //reset the input fields
       templateName.value="";
       templateDescription.value="";
       templateSubject.value="";
       templateBody.value="";
   }
</script>

<template>
     <div class="header-style">Email Templates</div>
     <div v-if="emailTemplates && emailTemplates.length > 0">
      <b-row v-for="(et, idx) in emailTemplates" :key="et.id">
            <b-col class="col-sm-4">
                <h6 >{{et.name}}</h6>
            </b-col>
            <b-col class="col-sm-6">
            <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" @click="editTemplate(idx)" />
            <font-awesome-icon icon="fa-solid fa-circle-xmark" class="fa-icon" @click="removeTemplate(idx)" />
       
            </b-col>
        </b-row>
        </div>
   
   <font-awesome-icon icon="fa-solid fa-circle-plus" @click="openPopUp()"/>Add New Email Template

    <ConfirmPopUp v-if="isAddNewTemplate || isEditTemplate" >
                <template v-slot:header>
                    Email Template
                    <button type="button"
                            class="btn-close"
                            @click="isAddNewTemplate=false; isEditTemplate=false;resetFields()">
                       
                    </button>
                </template>
                <template v-slot:body>
                    <b-row>
                        <div>Name : </div>
                        <div>
                            <b-form-input type="text" v-model="templateName" /> 
                          <!--  <span :class="requiredField">* Name is required </span>-->
                        </div>
                    </b-row>
                    <b-row>
                        <div>Description : </div>
                        <div><b-form-textarea  v-model="templateDescription" rows="2" ></b-form-textarea></div>
                    </b-row>
                    
                    <b-row>
                        <div>Subject : </div>
                        <div>
                            <b-form-input type="text" v-model="templateSubject" /> 
                          <!--  <span :class="requiredField">* Name is required </span>-->
                        </div>
                    </b-row>
                    <b-row>
                        <div>Body : </div>
                        <div><b-form-textarea  v-model="templateBody" rows="2" ></b-form-textarea></div>
                    </b-row>
                </template>
                <template v-slot:footer>
                    <button type="button"
                            class="modal-cancel-btn" 
                            @click="isAddNewTemplate=false; isEditTemplate=false;resetFields()"  
                                 
                            aria-label="Close modal">
                        Cancel
                    </button>
                    
                    <button type="button"
                            class="modal-confirm-btn"
                            style="margin-left:10px"
                           @click="addTemplate()"
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
