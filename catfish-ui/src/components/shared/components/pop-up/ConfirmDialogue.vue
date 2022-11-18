<script setup lang="ts">
    import { default as ConfirmPopUp } from './ConfirmPopUp.vue';
    import { ConfirmationPopUpModel } from './models';
    import { watch } from "vue";

    const props = defineProps<{
        model: ConfirmationPopUpModel,
    }>();
    
</script>
<template>
    <popup-modal ref="popup">
        <h2 style="margin-top: 0">Header</h2>
        <p>This is modal boady</p>
        <div class="btns">
            <button class="cancel-btn" @click="_cancel">Cancel</button>
            <span class="ok-btn" @click="_confirm">Ok</span>
        </div>
    </popup-modal>
</template>

<style scoped>
    .btns {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
    }

    .ok-btn {
        color: red;
        text-decoration: underline;
        line-height: 2.5rem;
        cursor: pointer;
    }

    .cancel-btn {
        padding: 0.5em 1em;
        background-color: #d5eae7;
        color: #35907f;
        border: 2px solid #0ec5a4;
        border-radius: 5px;
        font-weight: bold;
        font-size: 16px;
        text-transform: uppercase;
        cursor: pointer;
    }
</style>