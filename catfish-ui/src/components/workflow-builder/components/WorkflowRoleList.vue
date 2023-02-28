<script setup lang="ts">
    import { WorkflowRole, UserInfo} from '../models'
    import { computed, ref,watch } from 'vue';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { useWorkflowBuilderStore } from '../store';
    import { Guid } from 'guid-typescript';

    
    let role = ref({} as unknown as WorkflowRole)
    const store = useWorkflowBuilderStore();
    const addRoles = ref(false);
    const editMode = ref(false);
    let disabled = ref(true);
    const roles = ref(store.workflow?.roles);
    const selectedUserId = ref(Guid.EMPTY  as unknown as Guid);
    const ToggleAddRoles = () => (addRoles.value = !addRoles.value);
    watch(() => role.value.name, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })

    const addRole = (id: Guid) => {
        if(id === Guid.EMPTY as unknown as Guid){
            let newWorkflowRole = {
                id : Guid.create().toString() as unknown as Guid,
                name : role.value.name,
                description : role.value.description,
                users : role.value.users
            } as WorkflowRole;
    
        roles.value?.push(newWorkflowRole);
        }else{
            roles.value!.forEach((rl) => {
                if(rl.id === role.value.id){
                    rl.name = role.value.name;
                    rl.description = role.value.description;
                    rl.users = role.value.users;
                }    
             })
        }
        
        resetFields();
        editMode.value = false;
        addRoles.value = false;
    }
    const addUser = (id: Guid) => {
        if(id != Guid.EMPTY as unknown as Guid){
            const newUser = store.users.find(u => u.id == id) as UserInfo;
            role.value.users.push(newUser);
        }
    }
    const resetFields = () => {
        role.value.id = Guid.EMPTY as unknown as Guid;
        role.value.name = "";
        role.value.description = "";
        role.value.users = [];
        editMode.value = false;
    }

    const deleteRole = (roleId: Guid) => {
        const idx = roles.value?.findIndex(opt => opt.id == roleId)
        roles.value?.splice(idx as number, 1)
    }
    const deleteUser = (userId: Guid) => {
        const idx = role.value.users?.findIndex(usr => usr.id == userId)
        role.value.users?.splice(idx as number, 1)
    }
    const editRole = (editRoleId: Guid) => {
        const roleValues = roles.value?.filter(opt => opt.id == editRoleId) as WorkflowRole[]
        role.value.name = roleValues[0].name 
        role.value.description = roleValues[0].description as string
        role.value.id = roleValues[0].id
        role.value.users = roleValues[0].users
        editMode.value = true
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
                        <b-form-input v-model="role.name" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Description" class="mt-3">
                        <b-form-textarea v-model="(role.description as string)" rows="3" max-rows="6"></b-form-textarea>
                    </b-input-group>
                    <div class="content-style">Users</div>
                    <div class="popup-list-item">
                        <b-list-group>
                            <b-list-group-item v-for="user in role.users" :key="user.userName">
                                <span>{{user.userName}}
                                    <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteUser(user.id as Guid)"/>
                                </span>
                            </b-list-group-item>
                        </b-list-group>
                    </div>
                    <b-input-group prepend="Users" class="mt-12">
                        <select class="form-select" v-model="selectedUserId">
                            <option v-for="user in store.users" :value="user.id">{{user.userName}}</option>
                        </select>
                        <span class="trigger-add"><font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#00cc66" @click="addUser(selectedUserId as unknown as Guid)"/></span>
                    </b-input-group>
                </div>
            </template>
            <template v-slot:footer>
                <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addRole(role.id as Guid)"><span v-if="!editMode">Add</span><span v-if="editMode">Update</span></button>
            </template>
        </ConfirmPopUp>
    </div>
</template>