<script setup lang="ts">
    import { Workflow } from '../models/'
    import { computed, ref } from 'vue';
    import {default as ConfirmPopUp} from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { useWorkflowBuilderStore } from '../store';

    const store = useWorkflowBuilderStore();
    const addRoles = ref(false);
    const roleName = ref("");
    const roleDescription = ref("");
    const ToggleAddRoles = () => (addRoles.value = !addRoles.value);
</script>

<template>
    <div>
        <div class="header-style">Roles <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="ToggleAddRoles()"/></div>
        <ConfirmPopUp v-if="addRoles" >
            <template v-slot:header>
                Create a Role.
                <button type="button"
                        class="btn-close"
                        @click="addRoles=false">
                    x
                </button>
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
                <button type="button"
                        class="modal-add-btn"
                        aria-label="Close modal">
                    Add
                </button>
                
            </template>
        </ConfirmPopUp>
    </div>
</template>