<script setup lang="ts">
    import { ref, watch,computed } from 'vue';
    import { eEmailType, eTriggerType ,eRecipientType} from "../../../components/shared/constants";
    import { useWorkflowBuilderStore } from '../store';
    import { WorkflowTrigger, Recipient } from '../models'
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';

    const props = defineProps < { visibility: boolean } > ();
    const store = useWorkflowBuilderStore();
    let   showPannel = ref(props.visibility);
    const triggerId = ref("");
    const triggerType = ref("");
    const triggerName = ref("");
    const triggerDescription = ref("");
    const selectedEmailTemplate = ref("");
    const recipientId = ref("");
    const emailType = ref("");
    const reciepientType = ref("");
    const role = ref("");
    const email = ref("");
    const addRecipients = ref(false);
    const showRole = ref(false);
    const showEmail = ref(false);
    const showFormField = ref(false);
    const showMetadataField = ref(false);
    let   disabled = ref(true);
    const triggerTypes = computed(() => eTriggerType);
    const emailTypes = computed(() => eEmailType);
    const recipientTypes = computed(() => eRecipientType);
    const emailTemplates = computed(() => store.emailTemplates);
    const roleList = computed(() => store.roles);
    let toRecipients = computed(() => store.recipients?.filter(rec => rec.emailType == eEmailType.To) as Recipient[]);
    let ccRecipients = computed(() => store.recipients?.filter(rec => rec.emailType == eEmailType.Cc) as Recipient[]);
    let bccRecipients = computed(() => store.recipients?.filter(rec => rec.emailType == eEmailType.Bcc) as Recipient[]);

    watch(() => reciepientType.value, async newValue => {
        if (newValue  === eRecipientType.Role.toString()){
            showRole.value = true;
            showEmail.value = false;
            showFormField.value = false;
            showMetadataField.value = false;
        }else if (newValue === eRecipientType.Email.toString()){
            showEmail.value = true; 
            showRole.value = false;
            showFormField.value = false;
            showMetadataField.value = false;
        }else if (newValue === eRecipientType.FormField.toString()){
            showFormField.value = true;
            showRole.value = false; 
            showEmail.value = false;
            showMetadataField.value = false;
        }
        else if (newValue === eRecipientType.MetadataField.toString()){
            showMetadataField.value = true;
            showFormField.value = false;
            showRole.value = false; 
            showEmail.value = false;
        }else if (newValue === eRecipientType.Owner.toString()){
            showMetadataField.value = false;
            showFormField.value = false;
            showRole.value = false; 
            showEmail.value = false;
        }
    })

    const addTrigger = (id:string)=>{
        if(id.length === 0){
            let newState= {
                id:Guid.create(),
                type: triggerType.value as unknown as eTriggerType,
                name :triggerName.value,
                description : triggerDescription.value,
                templateId : selectedEmailTemplate.value as unknown as Guid,
                recipients : store.recipients
            } as WorkflowTrigger;
        
            store.triggers?.push(newState);
            store.recipients=null;
            resetFields()
        }
        }
    const resetFields =()=>{
        triggerType.value="";
        triggerName.value = "";
        triggerDescription.value = "";
        selectedEmailTemplate.value = "";
    }

    const resetRecipients =()=>{
        emailType.value="";
        reciepientType.value = "";
        role.value = "";
        email.value = "";
    }
    const addRecipient =(id: string)=>{
        if(id.length === 0){
        let newRecipient={
            id:Guid.create(),
            emailType: emailType.value as unknown as eEmailType,
            recipienType: reciepientType.value,
            role:role.value,
            email:email.value
        } as unknown as Recipient
        store.recipients?.push(newRecipient);
        addRecipients.value = false;
        resetRecipients();
    }
    }
    const deleteRecipient =(id: string)=>{
        const idx = store.recipients?.findIndex(opt => opt.id.toString() == id)
        store.recipients?.splice(idx as number, 1)
    }
    

    const ToggleAddRecipients = () => (addRecipients.value = !addRecipients.value)
        
    
</script>

<template>
    {{ store.triggers }}
    <div v-if="showPannel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <b-input-group prepend="Type" class="mt-3">
                <b-form-select v-model="triggerType" :options="triggerTypes"></b-form-select>
            </b-input-group>
            <b-input-group prepend="Name" class="mt-3">
                <b-form-input v-model="triggerName" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="triggerDescription" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
            <b-input-group prepend="Email Template" class="mt-3">
                <select class="form-select" v-model="selectedEmailTemplate">
                    <option v-for="opt in emailTemplates" >{{opt.name}}</option>
                </select>
            </b-input-group>
            <div class="title-recipient"><h5>To</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in toRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{recipient.role}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id.toString())"/>
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="title-recipient"><h5>Cc</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in ccRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{recipient.role}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id.toString())"/>
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="title-recipient"><h5>Bcc</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in bccRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{recipient.role}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id.toString())"/>
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="header-style">Recipients <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetRecipients();ToggleAddRecipients()"/></div>
           <ConfirmPopUp v-if="addRecipients" >
                <template v-slot:header>
                    Add a Recipient.
                    <button type="button" class="btn-close" @click="addRecipients=false">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="Email Type" class="mt-3">
                        <b-form-select v-model="emailType" :options="emailTypes"></b-form-select>
                    </b-input-group>
                    <b-input-group prepend="Recipient Type" class="mt-3">
                        <b-form-select v-model="reciepientType" :options="recipientTypes"></b-form-select>
                    </b-input-group>

                    <div v-if="showRole">
                        <b-input-group prepend="Role" class="mt-3">
                            <select class="form-select" v-model="role">
                                <option v-for="role in roleList" >{{role.name}}</option>
                            </select>
                        </b-input-group>
                    </div>
                    <div v-if="showEmail">
                        <b-input-group  prepend="Email" class="mt-3">
                            <b-form-input v-model="email" ></b-form-input>
                        </b-input-group>
                    </div>
                    <div v-if="showFormField">
                        Form Field here
                    </div>
                    <div v-if="showMetadataField">
                        Metadata Field here
                    </div>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addRecipient(recipientId)">Add recipient</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addTrigger(triggerId)">Add</button>
            </div>
            
        </div>

    </div>
</template>