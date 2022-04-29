<script lang="ts">
    import { defineComponent, PropType, ref } from 'vue'
    export default defineComponent({
        name: "Popups",
        components: {

        },
        props: {
            title: {
                type: null as PropType<string> | null,
                required: false,
                default: "Click me"
            },
           
            popup: {
                type: null as PropType<boolean> | null,
                required: false,
               
            }

        },

        setup(p) {
            const ispopup = ref(p.popup as boolean);
            console.log("inside popup: " + JSON.stringify(p.popup));
            return {
                ispopup
            }
        },

        methods: {
            closePopup: function () {
                this.ispopup = false;
            }
        }
    });
</script>

<template>
    <div class="popup" v-if="ispopup">
        <div class="popup-inner">
            <button class="popup-close" @click="closePopup()">
                Close Popup
            </button>
            <slot />

        </div>

    </div>
</template>

<style scoped>
    .popup {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 99;
        background-color: rgba(0, 0, 0, 0.2);
        display: flex;
        align-items: center;
        justify-content: center; 
    }

    .popup-inner {
        background: #FFF;
        padding: 32px;
        width:50%;
    }
</style>