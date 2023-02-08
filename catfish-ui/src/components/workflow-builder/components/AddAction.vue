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
    const action = ref({} as unknown as WorkflowAction);
    const button = ref({} as unknown as Button);
    const authorization = ref({} as Authorization);
    const buttons = ref([] as unknown as Button[]);
    const authorizations = ref([] as Authorization[]);
    const selectedTriggerId = ref(Guid.EMPTY  as unknown as Guid);
    const getField = (formId : Guid) => (store.entityTemplate?.forms.filter(f => f.id == formId)[0]);
    if(props.editMode){
      const actuionValues = store.workflow?.actions?.filter(a => a.id == props.editActionId ) as WorkflowAction[];
      action.value.id = actuionValues[0].id;
      action.value.name = actuionValues[0].name;
      action.value.description = actuionValues[0].description as string;
      action.value.formTemplateId = actuionValues[0].formTemplateId;
      store.loadTemplate(actuionValues[0].formTemplateId as Guid)
      action.value.formView = actuionValues[0].formView;
      actuionValues[0].buttons!.forEach((b) => {
          let newButton = {
          id : b.id,
          type : b.type,
          label : b.label,
          currentStateId : b.currentStateId,
          nextStateId : b.nextStateId,
          popupId : b.popupId, 
          triggers : b.triggers
          }  as Button
      buttons.value!.push(newButton);  
      });
      actuionValues[0].authorizations!.forEach((a) => {
          let newAuth = {
          id : a.id,
          currentStateId : a.currentStateId,
          authorizedBy : a.authorizedBy,
          authorizedRoleId : a.authorizedRoleId,
          authorizedDomain : a.authorizedDomain,
          authorizedFormId : a.authorizedFormId,
          authorizedFeildId : a.authorizedFeildId,
          authorizedMetadataFormId : a.authorizedMetadataFormId,
          authorizedMetadataFeildId : a.authorizedMetadataFeildId
          }  as Authorization
      authorizations.value!.push(newAuth);  
      })
    }else{
        action.value.id = Guid.EMPTY as unknown as Guid;
        authorization.value.id = Guid.EMPTY as unknown as Guid;
        button.value.id = Guid.EMPTY as unknown as Guid;
        button.value.triggers = [];
    }
    const getState = (stateId : Guid) => (store.workflow?.states.filter(st => st.id == stateId)[0]?.name);
    const getRole = (roleId : Guid) => (store.workflow?.roles.filter(r => r.id == roleId)[0]?.name);
    const getTrigger = (triggerId : Guid) => (store.workflow?.triggers.filter(tr => tr.id == triggerId)[0]?.name);
    const deletePanel = () => (store.showActionPanel = false);
    const addTrigger = (id : Guid) => {if(id != Guid.EMPTY as unknown as Guid)button.value.triggers.push(id)};
    const resetAuthFields = () => {
        authorization.value.id = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedBy = eAuthorizedBy.Owner;
        authorization.value.authorizedRoleId = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedFormId = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedFeildId = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedMetadataFormId = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedMetadataFeildId = Guid.EMPTY as unknown as Guid;
        authorization.value.authorizedDomain = "";
    }
    const resetButtonFields = () => {
        button.value.id = Guid.EMPTY as unknown as Guid;
        button.value.type = eButtonTypes.Button;
        button.value.label =  "";
        button.value.currentStateId = Guid.EMPTY as unknown as Guid;
        button.value.nextStateId = Guid.EMPTY as unknown as Guid;
        button.value.popupId = Guid.EMPTY as unknown as Guid;
        selectedTriggerId.value = Guid.EMPTY as unknown as Guid;
        button.value.triggers = [];
    }
    const resetFields = () => {
        action.value.name = "";
        action.value.description = "";
        action.value.formTemplateId = Guid.EMPTY as unknown as Guid;
        action.value.formView = eFormView.EntrySlip;
        buttons.value = [];
        authorizations.value = [];
    }
    const addButton = (id : Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
        let newButton = {
            id : Guid.create().toString() as unknown as Guid,
            type : button.value.type,
            label : button.value.label,
            currentStateId : button.value.currentStateId,
            nextStateId : button.value.nextStateId,
            popupId : button.value.popupId,
            triggers : button.value.triggers
        } as unknown as Button
        buttons.value?.push(newButton);
        }else{
            buttons.value!.forEach((b) => {
                if(b.id === id){
                    b.type = button.value.type,
                    b.label = button.value.label,
                    b.currentStateId = button.value.currentStateId,
                    b.nextStateId = button.value.nextStateId,
                    b.popupId = button.value.popupId,
                    b.triggers = button.value.triggers
                }
            })
        }
        addButtons.value = false;
        resetButtonFields();
    }
    const addAction = (id : Guid) => {
        if(id == Guid.EMPTY as unknown as Guid){
        let newAction = {
            id : Guid.create().toString() as unknown as Guid,
            name : action.value.name,
            description : action.value.description,
            formTemplateId : action.value.formTemplateId,
            formView : action.value.formView,
            buttons : buttons.value,
            authorizations : authorizations.value
        } as unknown as WorkflowAction
        store.workflow?.actions?.push(newAction);
        }else{
            store.workflow?.actions!.forEach((a)=> {
                if(a.id === id){
                    a.name = action.value.name,
                    a.description = action.value.description,
                    a.formTemplateId = action.value.formTemplateId,
                    a.formView = action.value.formView,
                    a.buttons = buttons.value,
                    a.authorizations = authorizations.value
                }
            })
        }
        store.showActionPanel = false;
        resetFields();
    }
    const addAuthorization = (id: Guid) => {
        if(id === Guid.EMPTY as unknown as Guid){
        let newAuth = {
            id : Guid.create().toString() as unknown as Guid,
            currentStateId : authorization.value.currentStateId,
            authorizedBy : authorization.value.authorizedBy,
            authorizedRoleId : authorization.value.authorizedRoleId,
            authorizedDomain : authorization.value.authorizedDomain,
            authorizedFormId : authorization.value.authorizedFormId,
            authorizedFeildId : authorization.value.authorizedFeildId,
            authorizedMetadataFormId : authorization.value.authorizedMetadataFormId,
            authorizedMetadataFeildId : authorization.value.authorizedMetadataFeildId
            
        } as unknown as Authorization
        authorizations.value?.push(newAuth);
        }
        
        resetAuthFields();
        addAuthorizations.value = false;
        resetAuthFields();
    }
    const deleteAuthorization = (id : Guid) => {
        const idx = authorizations.value?.findIndex(auth => auth.id == id)
        authorizations.value?.splice(idx as number, 1)
    }
    const deleteButton = (id : Guid) => {
        const idx = buttons.value?.findIndex(btn => btn.id == id)
        buttons.value?.splice(idx as number, 1)
    }
    const deleteTrigger = (id : Guid) => {
        const idx = button.value.triggers.findIndex(tr => tr == id)
        button.value.triggers?.splice(idx as number, 1)
    }
    const editButton = (btnId : Guid) => {
        const buttonValues = buttons.value?.filter(btn => btn.id == btnId) as Button[]
        button.value.id = buttonValues[0].id
        button.value.type = buttonValues[0].type 
        button.value.label = buttonValues[0].label
        button.value.currentStateId = buttonValues[0].currentStateId
        button.value.nextStateId = buttonValues[0].nextStateId
        button.value.popupId = buttonValues[0].popupId
        button.value.triggers = buttonValues[0].triggers
        addButtons.value = true;
    }
</script>

<template>
    <div v-if="store.showActionPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <div class="panel-delete">
                <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deletePanel()"/>
            </div>
            <b-input-group prepend="Name" class="mt-3">
                <b-form-input v-model="action.name" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Description" class="mt-3">
                <b-form-textarea v-model="(action.description as string)" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>

            <div class="header-style">Submission Forms</div>
            <b-input-group prepend="Form Template" class="mt-3">
                <select class="form-select" v-model="action.formTemplateId">
                    <option v-for="form in store.entityTemplate?.entityTemplateSettings?.dataForms" :value="form.id">{{ form.name }}</option>
                </select>
            </b-input-group>
            <b-input-group prepend="Form View" class="mt-3">
                <b-form-select v-model="action.formView" :options="eFormView"></b-form-select>
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
                        <select class="form-select" v-model="button.type">
                            <option v-for="button in eByttonTypeValues" :value="button">{{getButtonTypeLabel(button)}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Label" class="mt-3">
                        <b-form-input v-model="button.label" ></b-form-input>
                    </b-input-group>
                    <div class="content-style">Conditions</div>
                    <b-input-group prepend="For State" class="mt-3">
                        <select class="form-select" v-model="button.currentStateId">
                            <option v-for="forState in store.workflow?.states" :value="forState.id">{{forState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="New State" class="mt-3">
                        <select class="form-select" v-model="button.nextStateId">
                            <option v-for="nextState in store.workflow?.states" :value="nextState.id">{{nextState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Pop-up" class="mt-3">
                        <select class="form-select" v-model="button.popupId">
                            <option v-for="popup in store.workflow?.popups" :value="popup.id">{{popup.title}}</option>
                        </select>
                    </b-input-group>
                    <div class="content-style">Triggers</div>
                    <div class="popup-list-item">
                        <b-list-group>
                            <b-list-group-item v-for="triggerId in button.triggers" :key="triggerId.toString()">
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
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(button.id as Guid)">Add Button</button>
                </template>
            </ConfirmPopUp>
            <div v-if="authorizations.length>0" class="header-style">Authorizations</div>
            <div class="popup-list-item">
                <b-list-group>
                    <b-list-group-item v-for="auth in authorizations" :key="(auth.id as unknown as string)">
                        <span>{{ getState(auth.currentStateId as Guid) }}-</span><span>{{getRole(auth.authorizedRoleId as Guid)}}</span><span>{{auth.authorizedDomain}}</span><span v-if="auth.authorizedBy==eAuthorizedBy.Owner">Owner</span>
                        <span>
                            <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteAuthorization(auth.id as Guid)"/>
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
                        <select class="form-select" v-model="authorization.currentStateId" >
                            <option v-for="state in store.workflow?.states" :value="state.id">{{state.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Authorized By" class="mt-3">
                        <select class="form-select" v-model="authorization.authorizedBy">
                            <option v-for="auth in eAuthorizedByValues" :value="auth">{{getAuthorizedByLabel(auth)}}</option>
                        </select>
                    </b-input-group>
                    <div v-if="authorization.authorizedBy == eAuthorizedBy.Role">
                        <b-input-group prepend="Role" class="mt-3">
                        <select class="form-select" v-model="authorization.authorizedRoleId">
                            <option v-for="role in store.workflow?.roles" :value="role.id">{{role.name}}</option>
                        </select>
                    </b-input-group>
                    </div>
                    <div v-if="authorization.authorizedBy == eAuthorizedBy.Domain">
                        <b-input-group prepend="Domain" class="mt-3">
                            <b-form-input v-model="(authorization.authorizedDomain as string)" ></b-form-input>
                        </b-input-group>
                    </div>
                    <div v-if="authorization.authorizedBy == eAuthorizedBy.FormField">
                        <b-input-group  prepend="Form" class="mt-3">
                            <select class="form-select" v-model="authorization.authorizedFormId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.dataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Field" class="mt-3">
                            <select class="form-select" v-model="authorization.authorizedFeildId">
                                <option v-for="field in getField(authorization.authorizedFormId as Guid)?.fields" :value="field.id">{{field.title}}</option>
                            </select>
                        </b-input-group>
                    </div>
                    <div v-if="authorization.authorizedBy == eAuthorizedBy.MetadataField">
                        <b-input-group  prepend="Metadata Form" class="mt-3">
                            <select class="form-select" v-model="authorization.authorizedMetadataFormId">
                                <option v-for="form in store.entityTemplate?.entityTemplateSettings.metadataForms" :value="form.id">{{form.name}}</option>
                            </select>
                        </b-input-group>
                        <b-input-group  prepend="Metadata Field" class="mt-3">
                            <select class="form-select" v-model="authorization.authorizedMetadataFeildId">
                                <option v-for="field in getField(authorization.authorizedMetadataFormId as Guid)?.fields" :value="field.id">{{field.title}}</option>
                            </select>
                        </b-input-group>
                    </div>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAuthorization(authorization.id as Guid)">Add Authorization</button>
                </template>
            </ConfirmPopUp>
            <div style="margin-left: 90%;">
                <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addAction(action.id as Guid)">Add</button>
            </div>
        </div>
    </div>
</template>