<script setup lang="ts">
    import { WorkflowRoles } from '../models/'
    import { computed, ref,watch } from 'vue';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { useWorkflowBuilderStore } from '../store';
    import { Guid } from 'guid-typescript';

    const store = useWorkflowBuilderStore();
    const addRoles = ref(false);
    const roleName = ref("");
    const roleDescription = ref("");
    let disabled = ref(true);
    const ToggleAddRoles = () => (addRoles.value = !addRoles.value);
    watch(() => roleName.value, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })

    const addRole = ()=>{
        let _name = roleName.value;
        let _description = roleDescription.value;
        let newWorkflowRole= {
            id:Guid.create(),
            name :_name,
            description : _description
        } as WorkflowRoles;
    
        store.roles?.push(newWorkflowRole);
        roleName.value = "";
        roleDescription.value = "";
        addRoles.value = false;
    }
</script>

<template>
    <div>
        <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="role in store.roles" :key="role.name">
                    <span>{{role.name}}</span>
                    <span>
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;"/>
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
        <div class="header-style">Roles <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="ToggleAddRoles()"/></div>
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
                <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addRole">Add</button>
            </template>
        </ConfirmPopUp>
    </div>
</template>