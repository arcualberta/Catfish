<script lang="ts" setup>
import { Guid } from 'guid-typescript';
import { ListEntry } from '@/components/shared';
import { computed, ref } from 'vue';
import { default as config } from "@/appsettings";
    import { default as ConfirmPopUp } from '../../../components/shared/components/pop-up/ConfirmPopUp.vue';
import { useCRUDManagerStore } from './store'

const popupTrigger = ref(false);
const props = defineProps<{
    entry: ListEntry 
}>()

const store = useCRUDManagerStore();
const TogglePopup = () => (popupTrigger.value = !popupTrigger.value);
    const deleteEntry = (apiUrl: string) => {
        store.deleteObject(apiUrl);
        popupTrigger.value = !popupTrigger.value;
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
        <a href="#" class="col-2">Change State</a>
        <a @click="TogglePopup()" class="col-2 delete-link">Delete</a>
        <ConfirmPopUp v-if="popupTrigger" :popupTrigger="true">
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
                        class="modal-confirm-btn"
                        @click="deleteEntry('https://localhost:5020/api/items/'+ props.entry.id)"
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
        </ConfirmPopUp>
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
    
</style>