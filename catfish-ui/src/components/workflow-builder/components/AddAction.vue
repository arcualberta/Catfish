<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { ref, computed } from 'vue';
    import { Authorization, Button, WorkflowAction } from '../models';
    import { useWorkflowBuilderStore } from '../store';
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { eFormView, eTriggerType ,eByttonTypeValues, getButtonTypeLabel, getAuthorizedByLabel, eAuthorizedByValues, eAuthorizedBy, eButtonTypes} from "../../../components/shared/constants";
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
    import { BInputGroup, BFormInput, BFormTextarea, BFormSelect, BListGroup, BListGroupItem } from 'bootstrap-vue-3';
    
    const props = defineProps < { editMode: boolean,
                                  editActionId: string } > ();
    const store = useWorkflowBuilderStore();
    const addButtons = ref(false);
    const addAuthorizations = ref(false);
    const actionId = ref("");
    const actionName = ref("");
    const actionDescription = ref("");
    const formTemplateId = ref(Guid.EMPTY);
    const triggerTypes = computed(() => eTriggerType);
    const formView = ref("");
    const buttonId = ref("");
    const buttonType = ref("");
    const buttonLable = ref("");
    const currentStateId = ref("");
    const nextStateId = ref(Guid.EMPTY);
    const popupId = ref(Guid.EMPTY);
    const selectedTriggerId = ref(Guid.EMPTY);
    const triggerIds = ref([] as Guid[]);
    const buttons = ref([] as unknown as Button[]);
    const authorization = ref([] as Authorization[]);
    const authorizationId = ref(Guid.EMPTY);
    const authStateId = ref(Guid.EMPTY);
    const authorizedBy = ref("");
    const authRoleId = ref(Guid.EMPTY);
    const authFormId = ref(Guid.EMPTY);
    const authFieldId = ref(Guid.EMPTY);
    const authMetadataFormId = ref(Guid.EMPTY);
    const authMetadataFieldId = ref(Guid.EMPTY);
    const domain = ref("");
    const getState = (stateId:Guid) => (store.workflow?.states.filter(st=>st.id==stateId)[0]?.name);
    const getRole = (roleId:Guid) => (store.workflow?.roles.filter(r=>r.id==roleId)[0]?.name);
    const getTrigger = (triggerId:Guid) => (store.workflow?.triggers.filter(tr=>tr.id==triggerId)[0]?.name);
    const deletePanel = () => (store.showActionPanel=false);
    const addTrigger = (id: Guid) => (triggerIds.value.push(id));
    const resetAuthFields = () => {
        authStateId.value = Guid.EMPTY;
        authorizedBy.value = "";
        authRoleId.value = Guid.EMPTY;
        authFormId.value = Guid.EMPTY;
        authFieldId.value = Guid.EMPTY;
        authMetadataFormId.value = Guid.EMPTY;
        authMetadataFieldId.value = Guid.EMPTY;
        domain.value = "";
    }
    const resetButtonFields = () => {
        buttonId.value = "";
        buttonType.value = "";
        buttonLable.value ="";
        currentStateId.value = "";
        nextStateId.value = Guid.EMPTY;
        popupId.value = Guid.EMPTY;
        selectedTriggerId.value = Guid.EMPTY;
        triggerIds.value = [];
    }
    const resetFields = () => {
        actionName.value = "";
        actionDescription.value ="";
        formTemplateId.value = Guid.EMPTY;
        formView.value = "";
        buttons.value = [];
        authorization.value = [];
    }
    const addButton = (id: string) => {
        if(id.length === 0){
        let newButton = {
            id:Guid.create(),
            type: buttonType.value,
            lable: buttonLable.value,
            currentStateId: currentStateId.value as unknown as Guid,
            nextStateId: nextStateId.value,
            popupId: popupId.value,
            triggers:triggerIds.value
        } as unknown as Button
        buttons.value?.push(newButton);
        }else{
            console.log("edit save called")
            buttons.value!.forEach((b)=> {
                if(b.id.toString() === id){
                    b.type = buttonType.value as unknown as eButtonTypes,
                    b.lable= buttonLable.value,
                    b.currentStateId = currentStateId.value as unknown as Guid,
                    b.nextStateId = nextStateId.value as unknown as Guid,
                    b.popupId = popupId.value as unknown as Guid,
                    b.triggers = triggerIds.value
                }
            })
        }
        addButtons.value = false;
        resetButtonFields();
    }
    const addAction = (id: string) => {
        if(id.length === 0){
        let newAction = {
            id:Guid.create(),
            name: actionName.value,
            description: actionDescription.value,
            formTemplate: formTemplateId.value,
            formView: formView.value,
            buttons: buttons.value,
            authorizations: authorization.value
        } as unknown as WorkflowAction
        store.workflow?.actions?.push(newAction);
        store.showActionPanel = false;
        resetFields();
        }
    }
    const addAuthorization = (id: string) => {
        if(id.length === 0){
        let newAuth = {
            id:Guid.create(),
            currentState: authStateId.value,
            authorizedBy: authorizedBy.value,
            authorizedRoleId: authRoleId.value,
            authorizedDomain: domain.value,
            authorizedFormId: authFormId.value,
            authorizedFeildId: authFieldId.value,
            authorizedMetadataFormId: authMetadataFormId,
            authorizedMetadataFeildId: authMetadataFieldId
            
        } as unknown as Authorization
        authorization.value?.push(newAuth);
        addAuthorizations.value = false;
        resetAuthFields();
        }
    }
    const deleteAuthorization =(id: string)=>{
        const idx = authorization.value?.findIndex(auth => auth.id.toString() == id)
        authorization.value?.splice(idx as number, 1)
    }
    const deleteButton =(id: string)=>{
        const idx = buttons.value?.findIndex(btn => btn.id.toString() == id)
        buttons.value?.splice(idx as number, 1)
    }
    const deleteTrigger = (id: string)=>{
        const idx = triggerIds.value.findIndex(tr => tr.toString() == id)
        triggerIds.value?.splice(idx as number, 1)
    }
    const editButton = (btnId: Guid) => {
        const buttonValues = buttons.value?.filter(btn => btn.id == btnId) as Button[]
        buttonId.value = buttonValues[0].id as unknown as string
        buttonType.value=buttonValues[0].type as unknown as string
        buttonLable.value = buttonValues[0].lable
        currentStateId.value = buttonValues[0].currentStateId as unknown  as string
        nextStateId.value = buttonValues[0].nextStateId as unknown as string
        popupId.value = buttonValues[0].popupId as unknown as string
        triggerIds.value = buttonValues[0].triggers
        addButtons.value = true;
    }
    const editAuth = (authId: Guid) => {
        const authValues = authorization.value?.filter(au => au.id == authId) as Authorization[]
        authorizationId.value = authValues[0].id as unknown as string
        authStateId.value=authValues[0].currentState as unknown as string
        authorizedBy.value = authValues[0].authorizedBy as unknown as string
        authRoleId.value = authValues[0].authorizedRoleId as unknown  as string
        domain.value = authValues[0].authorizedDomain as unknown as string
        authFormId.value = authValues[0].authorizedFormId as unknown as string
        authFieldId.value = authValues[0].authorizedFeildId as unknown as string
        authMetadataFormId.value = authValues[0].authorizedMetadataFormId as unknown as string
        authMetadataFieldId.value = authValues[0].authorizedMetadataFeildId as unknown as string
        addAuthorizations.value = true;
    }
</script>

<template>
    <div v-if="store.showActionPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <div class="panel-delete">
                <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deletePanel()"/>
            </div>
            <b-input-group prepend="Name" class="mt-3">
                <b-form-input v-model="actionName" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="actionDescription" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>

            <div class="header-style">Submission Forms</div>
            <b-input-group prepend="Form Template" class="mt-3">
                <b-form-select v-model="formTemplateId" :options="triggerTypes"></b-form-select>
            </b-input-group>
            <b-input-group prepend="Form View" class="mt-3">
                <b-form-select v-model="formView" :options="eFormView"></b-form-select>
            </b-input-group>
            <div v-if="buttons.length > 0" class="header-style">Buttons</div>
            <div class="popup-list-item">
                <b-list-group>
                    <b-list-group-item v-for="btn in buttons" :key="(btn.id.toString())">
                        <span>{{ btn.lable }}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteButton(btn.id.toString())"/>
                            <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editButton(btn.id as Guid)" />
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            {{ buttons }}
            <div class="content-style">Add Button <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="addButtons = true"/></div>
            <ConfirmPopUp v-if="addButtons" >
                <template v-slot:header>
                    Add a Button.
                    <button type="button" class="btn-close" @click="addButtons=false;resetButtonFields()">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="Button type" class="mt-3">
                        <select class="form-select" v-model="buttonType">
                            <option v-for="button in eByttonTypeValues" :value="button">{{getButtonTypeLabel(button)}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Lable" class="mt-3">
                        <b-form-input v-model="buttonLable" ></b-form-input>
                    </b-input-group>
                    <div class="content-style">Conditions</div>
                    <b-input-group prepend="For State" class="mt-3">
                        <select class="form-select" v-model="currentStateId">
                            <option v-for="forState in store.workflow?.states" :value="forState.id">{{forState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="New State" class="mt-3">
                        <select class="form-select" v-model="nextStateId">
                            <option v-for="nextState in store.workflow?.states" :value="nextState.id">{{nextState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Pop-up" class="mt-3">
                        <select class="form-select" v-model="popupId">
                            <option v-for="popup in store.workflow?.popups" :value="popup.id">{{popup.title}}</option>
                        </select>
                    </b-input-group>
                    <div class="content-style">Triggers</div>
                    <div class="popup-list-item">
                        <b-list-group>
                            <b-list-group-item v-for="triggerId in triggerIds" :key="triggerId.toString()">
                                <span>{{getTrigger(triggerId as Guid)}}
                                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteTrigger(triggerId.toString())"/>
                                </span>
                            </b-list-group-item>
                        </b-list-group>
                    </div>
                    <b-input-group prepend="Triggers" class="mt-12">
                        <select class="form-select" v-model="selectedTriggerId">
                            <option v-for="trigger in store.workflow?.triggers" :value="trigger.id">{{trigger.name}}</option>
                        </select>
                        <span class="trigger-add"><font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#00cc66" @click="addTrigger(selectedTriggerId as unknown as Guid)"/></span>
                    </b-input-group>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(buttonId)">Add Button</button>
                </template>
            </ConfirmPopUp>
            <div v-if="authorization.length>0" class="header-style">Authorizations</div>
            <div class="popup-list-item">
                <b-list-group>
                    <b-list-group-item v-for="auth in authorization" :key="auth.authorizedBy">
                        <span>{{ getState(auth.currentState as Guid) }}-</span><span>{{getRole(auth.authorizedRoleId as Guid)}}</span><span>{{auth.authorizedDomain}}</span><span v-if="auth.authorizedBy==eAuthorizedBy.Owner">Owner</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAuthorization(auth.id.toString())"/>
                            <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editAuth(auth.id as Guid)" />
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
            <div class="content-style">Add Authorization <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="addAuthorizations = true"/></div>
            <ConfirmPopUp v-if="addAuthorizations" >
                <template v-slot:header>
                    Add a Authorization.
                    <button type="button" class="btn-close" @click="addAuthorizations=false">x</button>
                </template>
                <template v-slot:body>
                <div >
                    <b-input-group prepend="For state" class="mt-3">
                        <select class="form-select" v-model="authStateId" >
                            <option v-for="state in store.workflow?.states" :value="state.id">{{state.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Authorized By" class="mt-3">
                        <select class="form-select" v-model="authorizedBy">
                            <option v-for="auth in eAuthorizedByValues" :value="auth">{{getAuthorizedByLabel(auth)}}</option>
                        </select>
                    </b-input-group>
                    <div v-if="authorizedBy==eAuthorizedBy.Role.toString()">
                        <b-input-group prepend="Role" class="mt-3">
                        <select class="form-select" v-model="authRoleId">
                            <option v-for="role in store.workflow?.roles" :value="role.id">{{role.name}}</option>
                        </select>
                    </b-input-group>
                    </div>
                    <div v-if="authorizedBy==eAuthorizedBy.Domain.toString()">
                        <b-input-group prepend="Domain" class="mt-3">
                            <b-form-input v-model="domain" ></b-form-input>
                        </b-input-group>
                    </div>
                    <div v-if="authorizedBy==eAuthorizedBy.FormField.toString()">
                        Form Field
                    </div>
                    <div v-if="authorizedBy==eAuthorizedBy.MetadataField.toString()">
                        Metadata Form Field
                    </div>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAuthorization(buttonId)">Add Authorization</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAction(actionId)">Add</button>
            </div>
        </div>
    </div>
</template>