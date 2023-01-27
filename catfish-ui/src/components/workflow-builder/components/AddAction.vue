<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import { ref, watch,computed } from 'vue';
    import { Button, WorkflowAction } from '../models';
    import { useWorkflowBuilderStore } from '../store';
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { eFormView, eTriggerType ,eByttonTypeValues, getButtonTypeLabel} from "../../../components/shared/constants";
    
    const props = defineProps < { editMode: boolean,
                                  editActionId: string } > ();
    const store = useWorkflowBuilderStore();
    const addButtons = ref(false);
    const actionName = ref("");
    const actionDescription = ref("");
    const formTemplateId = ref(Guid);
    const triggerTypes = computed(() => eTriggerType);
    //const buttonTypes = computed(() => eButtonTypes);
    
    const formView = ref("");
    const buttonId = ref("");
    const buttonType = ref("");
    const buttonLable = ref("");
    const currentStateId = ref(Guid);
    const nextStateId = ref(Guid);
    const popupId = ref(Guid);
    const selectedTriggerId = ref(Guid);
    const triggerId = ref(Array<Guid>);

    const deletePanel =()=>{
        store.showActionPanel=false;
        
        //resetFields()
    }
    const addButton =(id: string)=>{
        if(id.length === 0){
        let newButton={
            id:Guid.create(),
            type: buttonType.value,
            lable: buttonLable.value,
            currentState: currentStateId.value,
            newState: nextStateId.value,
            popupId: popupId.value
        } as unknown as Button
        store.actionButtons?.push(newButton);
        addButtons.value = false;
        }
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
            <div class="header-style">Buttons</div>
                {{ store.actionButtons }}
            <div class="header-style">Add Button <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="addButtons = true"/></div>
            <ConfirmPopUp v-if="addButtons" >
                <template v-slot:header>
                    Add a Button.
                    <button type="button" class="btn-close" @click="addButtons=false">x</button>
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
                    <div class="header-style">Conditions</div>
                    <b-input-group prepend="For State" class="mt-3">
                        <select class="form-select" v-model="currentStateId">
                            <option v-for="forState in store.workflow?.states" >{{forState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="New State" class="mt-3">
                        <select class="form-select" v-model="nextStateId">
                            <option v-for="nextState in store.workflow?.states" >{{nextState.name}}</option>
                        </select>
                    </b-input-group>
                    <b-input-group prepend="Pop-up" class="mt-3">
                        <select class="form-select" v-model="popupId">
                            <option v-for="popup in store.workflow?.popups" >{{popup.title}}</option>
                        </select>
                    </b-input-group>
                    <div class="header-style">Triggers</div>
                    <b-input-group prepend="Triggers" class="mt-12">
                        <select class="form-select" v-model="selectedTriggerId">
                            <option v-for="trigger in store.workflow?.triggers" >{{trigger.name}}</option>
                        </select>
                        <span class="trigger-add"><font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#00cc66" @click=""/></span>
                    </b-input-group>
                </div>
                </template>
                <template v-slot:footer>
                    <button type="button" class="modal-add-btn" aria-label="Close modal"  @click="addButton(buttonId)">Add Button</button>
                </template>
            </ConfirmPopUp>
        </div>
    </div>
</template>