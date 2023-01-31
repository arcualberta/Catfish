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
                                  editActionId: Guid } > ();
    const store = useWorkflowBuilderStore();
    const addButtons = ref(false);
    const addAuthorizations = ref(false);
    const actionId = ref(Guid.EMPTY as unknown as Guid);
    const actionName = ref("");
    const actionDescription = ref("");
    const formTemplateId = ref(Guid.EMPTY  as unknown as Guid);
    const triggerTypes = computed(() => eTriggerType);
    const formView = ref("");
    const buttonId = ref(Guid.EMPTY as unknown as Guid);
    const buttonType = ref("");
    const buttonLabel = ref("");
    const currentStateId = ref(Guid.EMPTY as unknown as Guid);
    const nextStateId = ref(Guid.EMPTY  as unknown as Guid);
    const popupId = ref(Guid.EMPTY  as unknown as Guid);
    const selectedTriggerId = ref(Guid.EMPTY  as unknown as Guid);
    const triggerIds = ref([] as Guid[]);
    const buttons = ref([] as unknown as Button[]);
    const authorization = ref([] as Authorization[]);
    const authorizationId = ref(Guid.EMPTY  as unknown as Guid);
    const authStateId = ref(Guid.EMPTY  as unknown as Guid);
    const authorizedBy = ref("");
    const authRoleId = ref(Guid.EMPTY  as unknown as Guid);
    const authFormId = ref(Guid.EMPTY  as unknown as Guid);
    const authFieldId = ref(Guid.EMPTY  as unknown as Guid);
    const authMetadataFormId = ref(Guid.EMPTY  as unknown as Guid);
    const authMetadataFieldId = ref(Guid.EMPTY as unknown as Guid);
    const domain = ref("");
    if(props.editMode){
      const actuinValues = store.workflow?.actions?.filter(a => a.id == props.editActionId ) as WorkflowAction[];
      actionId.value=actuinValues[0].id;
      actionName.value=actuinValues[0].name;
      actionDescription.value=actuinValues[0].description as string;
      formTemplateId.value =actuinValues[0].formTemplate;
      formView.value = actuinValues[0].formView;
      actuinValues[0].buttons!.forEach((b)=> {
          let newButton={
          id: b.id,
          type: b.type,
          label: b.label,
          currentStateId: b.currentStateId,
          nextStateId: b.nextStateId,
          popupId: b.popupId,
          triggers: b.triggers
          }  as Button
      buttons.value!.push(newButton);  
      });
      actuinValues[0].authorizations!.forEach((a)=> {
          let newAuth={
          id: a.id,
          currentState: a.currentState,
          authorizedBy: a.authorizedBy,
          authorizedRoleId: a.authorizedRoleId,
          authorizedDomain: a.authorizedDomain,
          authorizedFormId: a.authorizedFormId,
          authorizedFeildId: a.authorizedFeildId,
          authorizedMetadataFormId: a.authorizedMetadataFormId,
          authorizedMetadataFeildId: a.authorizedMetadataFeildId
          }  as Authorization
      authorization.value!.push(newAuth);  
      })
  }
    const getState = (stateId:Guid) => (store.workflow?.states.filter(st=>st.id==stateId)[0]?.name);
    const getRole = (roleId:Guid) => (store.workflow?.roles.filter(r=>r.id==roleId)[0]?.name);
    const getTrigger = (triggerId:Guid) => (store.workflow?.triggers.filter(tr=>tr.id==triggerId)[0]?.name);
    const deletePanel = () => (store.showActionPanel=false);
    const addTrigger = (id: Guid) => {if(id != Guid.EMPTY as unknown as Guid)triggerIds.value.push(id)};
    const resetAuthFields = () => {
        authStateId.value = Guid.EMPTY as unknown as Guid;
        authorizedBy.value = "";
        authRoleId.value = Guid.EMPTY as unknown as Guid;
        authFormId.value = Guid.EMPTY as unknown as Guid;
        authFieldId.value = Guid.EMPTY as unknown as Guid;
        authMetadataFormId.value = Guid.EMPTY as unknown as Guid;
        authMetadataFieldId.value = Guid.EMPTY as unknown as Guid;
        domain.value = "";
    }
    const resetButtonFields = () => {
        buttonId.value = Guid.EMPTY as unknown as Guid;
        buttonType.value = "";
        buttonLabel.value ="";
        currentStateId.value = Guid.EMPTY as unknown as Guid;
        nextStateId.value = Guid.EMPTY as unknown as Guid;
        popupId.value = Guid.EMPTY as unknown as Guid;
        selectedTriggerId.value = Guid.EMPTY as unknown as Guid;
        triggerIds.value = [];
    }
    const resetFields = () => {
        actionName.value = "";
        actionDescription.value ="";
        formTemplateId.value = Guid.EMPTY as unknown as Guid;
        formView.value = "";
        buttons.value = [];
        authorization.value = [];
    }
    const addButton = (id: Guid) => {
        console.log("id value",id)
        if(id == Guid.EMPTY as unknown as Guid){
        let newButton = {
            id:Guid.create(),
            type: buttonType.value,
            lable: buttonLabel.value,
            currentStateId: currentStateId.value as unknown as Guid,
            nextStateId: nextStateId.value,
            popupId: popupId.value,
            triggers:triggerIds.value
        } as unknown as Button
        buttons.value?.push(newButton);
        }else{
            console.log("edit value")
            buttons.value!.forEach((b)=> {
                if(b.id === id){
                    b.type = buttonType.value as unknown as eButtonTypes,
                    b.label= buttonLabel.value,
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
    const addAction = (id: Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
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
        }else{
            store.workflow?.actions!.forEach((a)=> {
                if(a.id === id){
                    a.name = actionName.value,
                    a.description= actionDescription.value,
                    a.formTemplate = formTemplateId.value,
                    a.formView = formView.value,
                    a.buttons = buttons.value,
                    a.authorizations = authorization.value
                }
            })
        }
        store.showActionPanel = false;
        resetFields();
    }
    const addAuthorization = (id: Guid) => {
        console.log("id ", id)
        if(id === Guid.EMPTY as unknown as Guid){
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
        }
        else{
            deleteAuthorization(id);
            let newAuth = {
            id: id,
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
        }
        resetAuthFields();
        addAuthorizations.value = false;
        resetAuthFields();
    }
    const deleteAuthorization =(id: Guid)=>{
        console.log("delete auth")
        const idx = authorization.value?.findIndex(auth => auth.id == id)
        authorization.value?.splice(idx as number, 1)
    }
    const deleteButton =(id: Guid)=>{
        const idx = buttons.value?.findIndex(btn => btn.id == id)
        buttons.value?.splice(idx as number, 1)
    }
    const deleteTrigger = (id: Guid)=>{
        const idx = triggerIds.value.findIndex(tr => tr == id)
        triggerIds.value?.splice(idx as number, 1)
    }
    const editButton = (btnId: Guid) => {
        const buttonValues = buttons.value?.filter(btn => btn.id == btnId) as Button[]
        buttonId.value = buttonValues[0].id as unknown as Guid
        buttonType.value=buttonValues[0].type as unknown as string
        buttonLabel.value = buttonValues[0].label
        currentStateId.value = buttonValues[0].currentStateId as unknown  as Guid
        nextStateId.value = buttonValues[0].nextStateId as unknown as Guid
        popupId.value = buttonValues[0].popupId as unknown as Guid
        triggerIds.value = buttonValues[0].triggers
        addButtons.value = true;
    }
    const editAuth = (authId: Guid) => {
        const authValues = authorization.value?.filter(au => au.id == authId) as Authorization[]
        authorizationId.value = authValues[0].id 
        authStateId.value=authValues[0].currentState 
        authorizedBy.value = authValues[0].authorizedBy as unknown as string
        authRoleId.value = authValues[0].authorizedRoleId as unknown as Guid
        domain.value = authValues[0].authorizedDomain as unknown as string
        authFormId.value = authValues[0].authorizedFormId as unknown as Guid
        authFieldId.value = authValues[0].authorizedFeildId as unknown as Guid
        authMetadataFormId.value = authValues[0].authorizedMetadataFormId as unknown as Guid
        authMetadataFieldId.value = authValues[0].authorizedMetadataFeildId as unknown as Guid
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
                        <span>{{ btn.label }}</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteButton(btn.id as Guid)"/>
                            <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editButton(btn.id as Guid)" />
                        </span>
                    </b-list-group-item>
                </b-list-group>
            </div>
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
                    <b-input-group prepend="Label" class="mt-3">
                        <b-form-input v-model="buttonLabel" ></b-form-input>
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
                                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteTrigger(triggerId as Guid)"/>
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
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(buttonId as Guid)">Add Button</button>
                </template>
            </ConfirmPopUp>
            <div v-if="authorization.length>0" class="header-style">Authorizations</div>
            <div class="popup-list-item">
                <b-list-group>
                    <b-list-group-item v-for="auth in authorization" :key="(auth.id as unknown as string)">
                        <span>{{ getState(auth.currentState as Guid) }}-</span><span>{{getRole(auth.authorizedRoleId as Guid)}}</span><span>{{auth.authorizedDomain}}</span><span v-if="auth.authorizedBy==eAuthorizedBy.Owner">Owner</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAuthorization(auth.id as Guid)"/>
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
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAuthorization(authorizationId as Guid)">Add Authorization</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAction(actionId as Guid)">Add</button>
            </div>
        </div>
    </div>
</template>