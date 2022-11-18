<script setup lang="ts">
    const props = defineProps<{
        popupTrigger: boolean,
    }>();

</script>

<template>
    <transition v-if="popupTrigger" name="fade">
        <div class="popup-modal" >
            <div class="window">
                <header class="modal-header">
                    <slot name="header">
                    </slot>
                    <button type="button"
                            class="btn-close"
                            @click="popupTrigger=false">
                    </button>
                </header>
                <section class="modal-body">
                    <slot name="body">
                    </slot>
                </section>
                <footer class="modal-footer">
                    <slot name="footer">
                    </slot>
                </footer>
            </div>
        </div>
    </transition>
</template>

<style scoped>
    /* css class for the transition */
    .fade-enter-active,
    .fade-leave-active {
        transition: opacity 0.3s;
    }

    .fade-enter,
    .fade-leave-to {
        opacity: 0;
    }

    .popup-modal {
        background-color: rgba(0, 0, 0, 0.5);
        position: fixed;
        top: 0;
        bottom: 0;
        left: 0;
        right: 0;
        padding: 0.5rem;
        display: flex;
        align-items: center;
        z-index: 1;
    }

    .window {
        background: #fff;
        border-radius: 5px;
        box-shadow: 2px 4px 8px rgba(0, 0, 0, 0.2);
        max-width: 480px;
        margin-left: auto;
        margin-right: auto;
        padding: 1rem;
    }
    .modal-header,
    .modal-footer {
        padding: 15px;
        display: flex;
    }

    .modal-header {
        position: relative;
        border-bottom: 1px solid #eeeeee;
        color: #4AAE9B;
        justify-content: space-between;
    }
    .modal-body {
        position: relative;
        padding: 20px 10px;
    }

    /*.btn-close {
        position: absolute;
        top: 0;
        right: 0;
        border: none;
        font-size: 20px;
        padding: 10px;
        cursor: pointer;
        font-weight: bold;
        color: #4AAE9B;
        background: transparent;
    }*/
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