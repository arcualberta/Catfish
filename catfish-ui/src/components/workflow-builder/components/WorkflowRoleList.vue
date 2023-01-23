<script setup lang="ts">
    import { WorkflowRole, TabNavigationDefinition} from '../models'
    import { computed, ref,watch } from 'vue';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { useWorkflowBuilderStore } from '../store';
    import { Guid } from 'guid-typescript';

    
    let roleId = ref("")
    let roleName = ref("")
    let roleDescription = ref("")
    const store = useWorkflowBuilderStore();
    const addRoles = ref(false);
    let disabled = ref(true);
    const roles = ref(store.workflow?.roles);
    const ToggleAddRoles = () => (addRoles.value = !addRoles.value);
    watch(() => roleName.value, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })

    const addRole = (id: string)=>{
        if(id.length===0){
            let newWorkflowRole= {
                id:Guid.create(),
                name :roleName.value,
                description : roleDescription.value
            } as WorkflowRole;
    
        roles.value?.push(newWorkflowRole);
        }else{
            const idx = roles.value?.findIndex(r => r.id.toString() == roleId.value)
            roles.value!.forEach((rl)=> {
                if(rl.id.toString() === roleId.value){
                    rl.name= roleName.value;
                    rl.description= roleDescription.value;
                }    
             })
        }
        
        resetFields();
        addRoles.value = false;
    }
    const resetFields = ()=>{
        roleId.value = "";
        roleName.value = "";
        roleDescription.value = "";
    }

    const deleteRole = (roleId: Guid) => {
        const idx = roles.value?.findIndex(opt => opt.id == roleId)
        roles.value?.splice(idx as number, 1)
    }
    const editRole = (editRoleId: Guid) => {
        const roleValues = roles.value?.filter(opt => opt.id == editRoleId) as WorkflowRole[]
        roleName.value=roleValues[0].name 
        roleDescription.value = roleValues[0].description as string
        roleId.value = roleValues[0].id.toString()
        addRoles.value = true
    }
</script>

<template>
    <div>
        <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="role in roles" :key="role.name">
                    <span>{{role.name}}</span>
                    <span>
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteRole(role.id as Guid)"/>
                        <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editRole(role.id as Guid)" />
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
        <div class="header-style">Roles <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="resetFields();ToggleAddRoles()"/></div>
        <ConfirmPopUp v-if="addRoles" >
            <template v-slot:header>
                Create a Role.
                <button type="button" class="btn-close" @click="addRoles=false">x</button>
            </template>
            <template v-slot:body>
                <div >
                    <b-input-group prepend="Name" class="mt-3">
                        <b-form-input v-model="roleName" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Description" class="mt-3">
                        <b-form-textarea v-model="roleDescription" rows="3" max-rows="6"></b-form-textarea>
                    </b-input-group>
                </div>
            </template>
            <template v-slot:footer>
                <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addRole(roleId)">Add</button>
            </template>
        </ConfirmPopUp>
    </div>
</template>