<script setup lang="ts">
    import { ref, watch,computed } from 'vue';
    import { eButtonReturnType } from "../../../components/shared/constants";
    import { default as ConfirmPopUp } from "../../shared/components/pop-up/ConfirmPopUp.vue"
    import { Guid } from 'guid-typescript';
    import { useWorkflowBuilderStore } from '../store';
    import { WorkflowPopup, PopupButton } from '../models'

    const store = useWorkflowBuilderStore();
    const props = defineProps < { editMode: boolean,
                                    editTriggerId: string } > ();

    const popupId = ref("");
    const popupTitle = ref("");
    const popupMessage = ref("");
    const popupButtons = ref(store.popupButtons)
    const returnTypes = computed(() => eButtonReturnType);
</script>

<template>
    <div v-if="store.showPopupPanel" class="col-sm-6">
        <div class="alert alert-secondary" role="alert">
            <b-input-group prepend="Title" class="mt-3">
                <b-form-input v-model="popupTitle" ></b-form-input>
            </b-input-group>
            <b-input-group prepend="Message" class="mt-3">
                <b-form-textarea v-model="popupMessage" rows="3" max-rows="6"></b-form-textarea>
            </b-input-group>
        </div>
    </div>
  
</template>