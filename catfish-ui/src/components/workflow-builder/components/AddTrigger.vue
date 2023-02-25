<script setup lang="ts">
    import { ref, computed } from 'vue';
    import { eEmailType, eTriggerType ,eRecipientType, eRecipientTypeValues, getRecipientTypeLabel, getTriggerTypeLabel, 
            eTriggerTypeValues, eEmailTypeValues, getEmailTypeLabel} from "../../../components/shared/constants";
    import { useWorkflowBuilderStore } from '../store';
    import { WorkflowTrigger, Recipient } from '../models'
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { getFieldTitle } from '@/components/shared/form-helpers'
    import { Guid } from 'guid-typescript';
    import { Field } from '@/components/shared/form-models';

    const props = defineProps < { editMode: boolean,
                                  editTriggerId: Guid } > ();
    const store = useWorkflowBuilderStore();
    const trigger = ref({} as unknown as WorkflowTrigger);
    const recipient = ref({} as unknown as Recipient);
    const recipients = ref([] as unknown as Recipient[]);
    const selectedUserId = ref(Guid.EMPTY  as unknown as Guid);
    const addRecipients = ref(false);
    const emailTemplates = computed(() => store.workflow?.emailTemplates);
    const roleList = computed(() => store.workflow?.roles);
    let toRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.To) as Recipient[]);
    let ccRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.Cc) as Recipient[]);
    let bccRecipients = computed(() => recipients.value?.filter(rec => rec.emailType == eEmailType.Bcc) as Recipient[]);
    const getUsers = (roleId : Guid) => (store.workflow?.roles.filter(r => r.id == roleId)[0]?.users);
    const getUser = (userId : Guid) => (store.users.filter(u => u.id == userId)[0]?.userName);
    const addUser = (id : Guid) => {if(id != Guid.EMPTY as unknown as Guid)recipient.value.users.push(id)};
    const getRole = (roleId : Guid) => (store.workflow?.roles.filter(r => r.id == roleId)[0]?.name);
    const getField = (formId : Guid) => (store.entityTemplate?.forms.filter(f => f.id == formId)[0]);
    const getFormName = (formId : Guid) => (store.entityTemplate?.forms.filter(f => f.id == formId)[0].name);
    const getFieldName = (formId : Guid, fieldId : Guid) => (getField(formId)?.fields.filter(f => f.id == fieldId)[0] as Field);
    const resetFields = () => {
        trigger.value.name = "";
        trigger.value.description = "";
        trigger.value.type = eTriggerType.Email;
        trigger.value.templateId = Guid.EMPTY as unknown as Guid;
        resetRecipients()
    }

    const resetRecipients = () => {
        recipient.value.emailType = eEmailType.To;
        recipient.value.recipientType = eRecipientType.Owner;
        recipient.value.roleId = Guid.EMPTY as unknown as Guid;
        selectedUserId.value = Guid.EMPTY as unknown as Guid;
        recipient.value.email = "";
        recipient.value.formId = Guid.EMPTY as unknown as Guid;
        recipient.value.fieldId = Guid.EMPTY as unknown as Guid;
        recipient.value.metadataFormId = Guid.EMPTY as unknown as Guid;
        recipient.value.metadataFeildId = Guid.EMPTY as unknown as Guid;
        recipient.value.users = [];
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
            recipientType : rl.recipientType,
            roleId : rl.roleId as Guid,
            users : rl.users,
            email : rl.email,
            formId : rl.formId as Guid,
            fieldId : rl.fieldId as Guid,
            metadataFormId : rl.metadataFormId as Guid,
            metadataFeildId : rl.metadataFeildId as Guid
            }  as Recipient
        recipients.value!.push(newRecipient); 
        })
    }else{
        trigger.value.id = Guid.EMPTY as unknown as Guid;
        recipient.value.id = Guid.EMPTY as unknown as Guid;
        resetFields();
    }
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
    
    const addRecipient = (id : Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
        let newRecipient = {
            id : Guid.create().toString() as unknown as Guid,
            emailType : recipient.value.emailType,
            recipientType : recipient.value.recipientType,
            roleId : recipient.value.roleId,
            users: recipient.value.users,
            email : recipient.value.email,
            formId : recipient.value.formId,
            fieldId : recipient.value.fieldId,
            metadataFormId : recipient.value.metadataFormId,
            metadataFeildId : recipient.value.metadataFeildId
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
    const deleteUser = (id : Guid) => {
        const idx = recipient.value.users.findIndex(u => u == id)
        recipient.value.users?.splice(idx as number, 1)
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
                        <span v-if="recipient.recipientType==eRecipientType.Owner">Owner</span>
                        <span>{{getRole(recipient.roleId as Guid)}}</span> 
                        <span>{{recipient.email}}</span>
                        <span v-if="recipient.recipientType==eRecipientType.FormField"> ( Form: {{ getFormName(recipient.formId as Guid) }} - Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null)}})</span>
                        <span v-if="recipient.recipientType==eRecipientType.MetadataField">( Metadata Form: {{ getFormName(recipient.metadataFormId as Guid) }} - Metadata Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null) }})</span>
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
                        <span v-if="recipient.recipientType==eRecipientType.Owner">Owner</span>
                        <span>{{getRole(recipient.roleId as Guid)}}</span>
                        <span>{{recipient.email}}</span>
                        <span v-if="recipient.recipientType==eRecipientType.FormField">( Form: {{ getFormName(recipient.formId as Guid) }} - Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null) }})</span>
                        <span v-if="recipient.recipientType==eRecipientType.MetadataField">( Metadata Form: {{ getFormName(recipient.formId as Guid) }} - Metadata Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null) }})</span>
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
                        <span v-if="recipient.recipientType==eRecipientType.Owner">Owner</span>
                        <span>{{getRole(recipient.roleId as Guid)}}</span>
                        <span>{{recipient.email}}</span>
                        <span v-if="recipient.recipientType==eRecipientType.FormField">( Form: {{ getFormName(recipient.formId as Guid) }} - Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null) }})</span>
                        <span v-if="recipient.recipientType==eRecipientType.MetadataField">( Metadata Form: {{ getFormName(recipient.formId as Guid) }} - Metadata Field: {{ getFieldTitle(getFieldName(recipient.formId as Guid, recipient.fieldId as Guid), null) }})</span>
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
                        <select class="form-select" v-model="recipient.recipientType">
                            <option v-for="con in eRecipientTypeValues" :value="con">{{getRecipientTypeLabel(con)}}</option>
                        </select>
                    </b-input-group>

                    <div v-if="recipient.recipientType == eRecipientType.Role">
                        <b-input-group prepend="Role" class="mt-3">
                            <select class="form-select" v-model="recipient.roleId">
                                <option v-for="role in roleList" :value="role.id">{{role.name}}</option>
                            </select>
                        </b-input-group>
                        <div class="popup-list-item">
                        <b-list-group>
                            <b-list-group-item v-for="userId in recipient.users" :key="userId.toString()">
                                <span>{{getUser(userId as Guid)}}
                                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteUser(userId as Guid)"/>
                                </span>
                            </b-list-group-item>
                        </b-list-group>
                    </div>
                    <b-input-group prepend="Users" class="mt-3">
                        <select class="form-select" v-model="selectedUserId">
                            <option v-for="user in getUsers(recipient.roleId as Guid)" :value="user.id">{{user.userName}}</option>
                        </select>
                        <span class="trigger-add"><font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#00cc66" @click="addUser(selectedUserId as Guid)"/></span>
                    </b-input-group>
                    </div>
                    <div v-if="recipient.recipientType == eRecipientType.Email">
                        <b-input-group  prepend="Email" class="mt-3">
                            <b-form-input v-model="(recipient.email as string)" ></b-form-input>
                        </b-input-group>
                    </div>
                    <div v-if="recipient.recipientType == eRecipientType.FormField">
                        <b-input-group  prepend="Form" class="mt-3">
                            <select class="form-select" v-model="recipient.formId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.dataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Field" class="mt-3">
                            <select class="form-select" v-model="recipient.fieldId">
                                <option v-for="field in getField(recipient.formId as Guid)?.fields" :value="field.id">{{getFieldTitle(field as Field, null)}}</option>
                            </select>
                        </b-input-group>
                    </div>
                    <div v-if="recipient.recipientType == eRecipientType.MetadataField">
                        <b-input-group  prepend="Metadata Form" class="mt-3">
                            <select class="form-select" v-model="recipient.metadataFormId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.metadataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Metadata Field" class="mt-3">
                            <select class="form-select" v-model="recipient.metadataFeildId">
                                <option v-for="field in getField(recipient.metadataFormId as Guid)?.fields" :value="field.id">{{getFieldTitle(field as Field, null)}}</option>
                            </select>
                        </b-input-group>
                    </div>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addRecipient(recipient.id as Guid)">Add</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 85%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addTrigger(trigger.id as Guid)"><span v-if="!props.editMode">Add</span><span v-if="props.editMode">Update</span></button>
            </div>
            
        </div>

    </div>
</template>