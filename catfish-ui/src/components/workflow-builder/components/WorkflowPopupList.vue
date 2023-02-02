<script setup lang="ts">
    import { Guid } from 'guid-typescript';
    import {default as AddPopup} from './AddPopup.vue'
    import { useWorkflowBuilderStore } from '../store';
    import { ref } from 'vue';
    
    const store = useWorkflowBuilderStore();
    const editMode = ref(false);
    const popupId = ref(Guid.EMPTY as unknown as Guid);

    const Toggle = () => (store.showPopupPanel = true)
    const deleteTrigger = (popupId : Guid) => {
        const idx = store.workflow?.popups?.findIndex(pop => pop.id == popupId)
        store.workflow?.popups?.splice(idx as number, 1)
    }
    const editTrigger = (id : Guid) => {
        editMode.value = true;
        popupId.value = id;
        store.showPopupPanel = true;
    }
</script>

<template>
    <div class="list-item">
            <b-list-group>
                <b-list-group-item v-for="popup in store.workflow?.popups" :key="(popup.id.toString())">
                    <span>{{popup.title}}</span>
                    <span style="display:inline">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" style="color: red; float: right;" @click="deleteTrigger(popup.id as Guid)"/>
                        <font-awesome-icon icon="fa-solid fa-pen-to-square"  class="fa-icon" style="color: #007bff; float: right;" @click="editTrigger(popup.id as Guid)" />
                    </span>
                </b-list-group-item>
            </b-list-group>
        </div>
    <div class="header-style">Pop-ups <font-awesome-icon icon="fa-solid fa-circle-plus" style="color:#1ca5b8" @click="Toggle()"/></div>
    <div v-if="store.showPopupPanel">
        <AddPopup :editMode="editMode" :editPopupId="(popupId as Guid)"/>
    </div>
</template>