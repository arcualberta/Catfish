<script setup lang="ts">
    import { WorkflowRole, TabNavigationDefinition} from '../models/'
    import { computed, ref,watch } from 'vue';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { useWorkflowBuilderStore } from '../store';
    import { Guid } from 'guid-typescript';

    const props = defineProps < { model: WorkflowRole } > ();
    const store = useWorkflowBuilderStore();
    const addRoles = ref(false);
    let disabled = ref(true);
    const ToggleAddRoles = () => (addRoles.value = !addRoles.value);
    watch(() => props.model.name, async newValue => {
        if (newValue.length>0)
            disabled.value = false; 
        else
            disabled.value = true; 
    })

    const addRole = ()=>{
        let newWorkflowRole= {
            id:Guid.create(),
            name :props.model.name,
            description : props.model.description
        } as WorkflowRole;
    
        store.roles?.push(newWorkflowRole);
        props.model.name = "";
        props.model.description = "";
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
                        <b-form-input v-model="props.model.name" ></b-form-input>
                    </b-input-group>
                    <b-input-group prepend="Description" class="mt-3">
                        <b-form-textarea v-model="(props.model.description as string)" rows="3" max-rows="6"></b-form-textarea>
                    </b-input-group>
                </div>
            </template>
            <template v-slot:footer>
                <button type="button" class="modal-add-btn" aria-label="Close modal" :disabled="disabled" @click="addRole">Add</button>
            </template>
        </ConfirmPopUp>
    </div>
</template>