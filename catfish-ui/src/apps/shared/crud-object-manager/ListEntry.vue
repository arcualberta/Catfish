<script lang="ts" setup>
import { Guid } from 'guid-typescript';
    import { ListEntry } from '@/components/shared';
    import { eState } from '../../../components/shared/constants';
import { computed, ref } from 'vue';
import { default as config } from "@/appsettings";
    import { default as ConfirmPopUp } from '../../../components/shared/components/pop-up/ConfirmPopUp.vue';
import { useCRUDManagerStore } from './store'

    const popupTrigger = ref(false);
    const changeStateTrigger = ref(false);
const props = defineProps<{
    entry: ListEntry 
}>()

    const store = useCRUDManagerStore();
    const apiRoot = computed(() => store.apiRoot)
    
    const stateList = computed(() => eState);
    console.log('stateList', stateList);
    const TogglePopup = () => (popupTrigger.value = !popupTrigger.value);
    const ToggleChangeStatePopup = () => (changeStateTrigger.value = !changeStateTrigger.value);
    const deleteEntry = (apiUrl: string) => {
        store.deleteObject(apiUrl);
        popupTrigger.value = !popupTrigger.value;
        store.loadEntries(apiRoot?.value as string);

    };
    const changeStatus = (apiUrl: string, id:Guid, newStatus:  eState) => {
        store.changeStatus(apiUrl, id, newStatus);
        popupTrigger.value = !popupTrigger.value;
        store.loadEntries(apiRoot?.value as string);

    };

//API ROOT/read/{entry.id}
//
const detailUrl ="/read/" + props.entry.id;
const updateUrl ="/update/" + props.entry.id;
const deleteUrl = "/delete/" + props.entry.id;
const changeStateUrl="/change-state/" + props.entry.id
</script>

<template>
    
    <div class="row entryRow">
        <router-link :to="detailUrl" class="col-6">{{entry.name}}</router-link>
        <router-link :to="updateUrl" class="col-2">Update</router-link>
        <a @click="ToggleChangeStatePopup()" class="col-2 change-state-link">Change State</a>
        <ConfirmPopUp v-if="changeStateTrigger" >
            <template v-slot:header>
                Change State.
                <button type="button"
                        class="btn-close"
                        @click="changeStateTrigger=false">
                    x
                </button>
            </template>
            <template v-slot:body>
                Please select new State.
                {{props.entry.state}}
                <div class="col-sm-3">
                    <select class="form-select" v-model="props.entry.state">
                        <option v-for="opt in stateList">{{opt}}</option>
                    </select>
                </div>
            </template>
            <template v-slot:footer>
                <button type="button"
                        class="modal-confirm-btn"
                        @click="changeStatus( apiRoot + '/change-state/' + props.entry.id,props.entry.id, props.entry.state)"
                        aria-label="Close modal">
                    Confirm
                </button>
                <button type="button"
                        class="modal-cancel-btn"
                        @click="ToggleChangeStatePopup()"
                        aria-label="Close modal">
                    Cancel
                </button>
            </template>
        </ConfirmPopUp>
        <a @click="TogglePopup()" class="col-2 delete-link">Delete</a>
        <!--<ConfirmPopUp v-if="popupTrigger" >
            <template v-slot:header>
                Delete Confirmation.
                <button type="button"
                        class="btn-close"
                        @click="popupTrigger=false">
                    x
                </button>
            </template>
            <template v-slot:body>
                Do you want to delete this Item?
            </template>
            <template v-slot:footer>
                <button type="button"
                        class="modal-delete-btn"
                        @click="deleteEntry(apiRoot + '/' + props.entry.id)"
                        aria-label="Close modal">
                    Delete
                </button>
                <button type="button"
                        class="modal-cancel-btn"
                        @click="TogglePopup()"
                        aria-label="Close modal">
                    Cancel
                </button>
            </template>
        </ConfirmPopUp>-->
    </div>

</template>
<style scoped>
   .entryRow{
    border: 1px solid lightgray;
    padding: 7px;
   }
    .delete-link{
        color:red;
    }
    .delete-link:hover {
        text-decoration:underline;
        cursor:pointer;
    }
    .change-state-link {
        color: #007eaa;
    }

        .change-state-link:hover {
            text-decoration: underline;
            cursor: pointer;
        }
    
</style>