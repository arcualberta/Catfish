<script setup lang="ts">
    import { ref, computed } from 'vue';
    import { eEmailType, eTriggerType ,eRecipientType, eRecipientTypeValues, getRecipientTypeLabel, getTriggerTypeLabel, 
            eTriggerTypeValues, eEmailTypeValues, getEmailTypeLabel} from "../../../components/shared/constants";
    import { useWorkflowBuilderStore } from '../store';
    import { WorkflowTrigger, Recipient } from '../models'
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';

    const props = defineProps < { editMode: boolean,
                                  editTriggerId: Guid } > ();
    const store = useWorkflowBuilderStore();
    const trigger = ref({} as unknown as WorkflowTrigger);
    const recipient = ref({} as unknown as Recipient);
    const recipients = ref([] as unknown as Recipient[]);
    const addRecipients = ref(false);
    const emailTemplates = computed(() => store.workflow?.emailTemplates);
    const roleList = computed(() => store.workflow?.roles);
    let toRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.To) as Recipient[]);
    let ccRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.Cc) as Recipient[]);
    let bccRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.Bcc) as Recipient[]);
    const getRole = (roleId : Guid) => (store.workflow?.roles.filter(r => r.id == roleId)[0]?.name);
    const getField = (formId : Guid) => (store.entityTemplate?.forms.filter(f => f.id == formId)[0]);

    const addTrigger = (id : Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
            let newTrigger = {
                id : Guid.create().toString() as unknown as Guid,
                type : trigger.value.type,
                name :trigger.value.name,
                description : trigger.value.description,
                templateId : trigger.value.templateId,
                recipients : recipients.value
            } as WorkflowTrigger;
        
            store.workflow?.triggers?.push(newTrigger);
            
        }else{
            store.workflow?.triggers!.forEach((tr) => {
                if(tr.id == id){
                    tr.type = trigger.value.type,
                    tr.name = trigger.value.name,
                    tr.description = trigger.value.description,
                    tr.templateId = trigger.value.templateId,
                    tr.recipients = recipients.value as Recipient[]
                }    
            })
        }
        store.showTriggerPanel = false;
        recipients.value = [];
        resetFields()
    }
    const resetFields = () => {
        trigger.value.name = "";
        trigger.value.description = "";
        trigger.value.type = eTriggerType.Email;
        trigger.value.templateId = Guid.EMPTY as unknown as Guid;
        resetRecipients()
    }

    const resetRecipients = () => {
        recipient.value.emailType = eEmailType.To;
        recipient.value.recipienType = eRecipientType.Owner;
        recipient.value.roleId = Guid.EMPTY as unknown as Guid;
        recipient.value.email = "";
        recipient.value.FormId = null;
        recipient.value.FeildId = null;
        recipient.value.MetadataFormId = null;
        recipient.value.MetadataFeildId = null;
    }
    const addRecipient = (id : Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
        let newRecipient = {
            id : Guid.create().toString() as unknown as Guid,
            emailType : recipient.value.emailType,
            recipienType : recipient.value.recipienType,
            roleId : recipient.value.roleId,
            email : recipient.value.email,
            FormId : recipient.value.FormId,
            FeildId : recipient.value.FeildId,
            MetadataFormId : recipient.value.MetadataFormId,
            MetadataFeildId : recipient.value.MetadataFeildId
        } as unknown as Recipient
        recipients.value?.push(newRecipient);
        addRecipients.value = false;
        resetRecipients();
    }
    }
    const deleteRecipient = (id : Guid) => {
        const idx = recipients.value?.findIndex(opt => opt.id == id)
        recipients.value?.splice(idx as number, 1)
    }
    const deletePanel = () => {
        store.showTriggerPanel = false;
        recipients.value = [];
        resetFields()
    }

    const ToggleAddRecipients = () => {
        addRecipients.value = !addRecipients.value;
        recipient.value.id =  Guid.EMPTY as unknown as Guid;
    }   
    if(props.editMode){
        const triggerValues = store.workflow?.triggers?.filter(tr => tr.id == props.editTriggerId ) as WorkflowTrigger[];
        trigger.value.id = triggerValues[0].id;
        trigger.value.type = triggerValues[0].type;
        trigger.value.name = triggerValues[0].name;
        trigger.value.description = triggerValues[0].description as string
        trigger.value.templateId = triggerValues[0].templateId
        triggerValues[0].recipients!.forEach((rl) => {
            let newRecipient = {
            id : rl.id,
            emailType : rl.emailType ,
            recipienType : rl.recipienType,
            roleId : rl.roleId,
            email : rl.email,
            FormId : rl.FormId,
            FeildId : rl.FeildId,
            MetadataFormId : rl.MetadataFormId,
            MetadataFeildId : rl.MetadataFeildId
            }  as Recipient
        recipients.value!.push(newRecipient);  
        recipient.value.id = Guid.EMPTY as unknown as Guid;
        })
    }else{
        trigger.value.id = Guid.EMPTY as unknown as Guid;
        recipient.value.id = Guid.EMPTY as unknown as Guid;
        resetFields();
    }
</script>

<template>
    <div v-if="store.showTriggerPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <div class="panel-delete">
                <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="resetFields();deletePanel()"/>
            </div>
            <b-input-group prepend="Type" class="mt-3">
                <select class="form-select" v-model="trigger.type">
                    <option v-for="con in eTriggerTypeValues" :value="con">{{getTriggerTypeLabel(con)}}</option>
                </select>
            </b-input-group>
            <b-input-group prepend="Name" class="mt-3">
                <b-form-input v-model="trigger.name" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="(trigger.description as string)" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
            <b-input-group prepend="Email Template" class="mt-3">
                <select class="form-select" v-model="trigger.templateId">
                    <option v-for="opt in emailTemplates"  :value="opt.id">{{opt.name}}</option>
                </select>
            </b-input-group>
            <div class="title-recipient"><h5>To</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in toRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{getRole(recipient.roleId as Guid)}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id)"/>
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="title-recipient"><h5>Cc</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in ccRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{getRole(recipient.roleId as Guid)}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id)"/>
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="title-recipient"><h5>Bcc</h5></div>
            <div class="list-recipient">
                <b-list-group>
                    <b-list-group-item v-for="recipient in bccRecipients" >
                        <span v-if="recipient.recipienType==eRecipientType.Owner">Owner</span><span>{{getRole(recipient.roleId as Guid)}}</span><span>{{recipient.email}}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRecipient(recipient.id)"/>
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
                        <select class="form-select" v-model="recipient.emailType">
                            <option v-for="con in eEmailTypeValues" :value="con">{{getEmailTypeLabel(con)}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Recipient Type" class="mt-3">
                        <select class="form-select" v-model="recipient.recipienType">
                            <option v-for="con in eRecipientTypeValues" :value="con">{{getRecipientTypeLabel(con)}}</option>
                        </select>
                    </b-input-group>

                    <div v-if="recipient.recipienType == eRecipientType.Role">
                        <b-input-group prepend="Role" class="mt-3">
                            <select class="form-select" v-model="recipient.roleId">
                                <option v-for="role in roleList" :value="role.id">{{role.name}}</option>
                            </select>
                        </b-input-group>
                    </div>
                    <div v-if="recipient.recipienType == eRecipientType.Email">
                        <b-input-group  prepend="Email" class="mt-3">
                            <b-form-input v-model="(recipient.email as string)" ></b-form-input>
                        </b-input-group>
                    </div>
                    
                    <div v-if="recipient.recipienType == eRecipientType.FormField">
                        <b-input-group  prepend="Form" class="mt-3">
                            <select class="form-select" v-model="recipient.FormId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.dataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Field" class="mt-3">
                            <select class="form-select" v-model="recipient.FeildId">
                                <option v-for="field in getField(recipient.FormId as Guid)?.fields" :value="field.id">{{field.title}}</option>
                            </select>
                        </b-input-group>
                        
                    </div>
                    <div v-if="recipient.recipienType == eRecipientType.MetadataField">
                        <b-input-group  prepend="Metadata Form" class="mt-3">
                            <select class="form-select" v-model="recipient.MetadataFormId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.metadataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Metadata Field" class="mt-3">
                            <select class="form-select" v-model="recipient.MetadataFeildId">
                                <option v-for="field in getField(recipient.MetadataFormId as Guid)?.fields" :value="field.id">{{field.title}}</option>
                            </select>
                        </b-input-group>
                    </div>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addRecipient(recipient.id as Guid)">Add recipient</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addTrigger(trigger.id as Guid)">Add</button>
            </div>
            
        </div>

    </div>
</template>