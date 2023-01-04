<script setup lang="ts">
    
</script>

<template>
    <transition  name="fade">
        <div class="popup-modal" >
            <div class="window">
                <header class="modal-header">
                    <slot name="header">
                    </slot>
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
        width: 300px;
    }
    .modal-header,
    .modal-footer {
        padding: 15px;
        display: flex;
    }
    .modal-header {
        position: relative;
        border-bottom: 1px solid #eeeeee;
        justify-content: space-between;
        font-weight: bold;
        font-size: 16px;
        color: #168C86;
    }
    .modal-body {
        position: relative;
        padding: 20px 10px;
    }
    
</style>