<script lang="ts">
    import { defineComponent, computed, PropType, ref } from 'vue'
    import { useStore } from 'vuex';
    import props from '../../props'
    import { State } from '../../../keyword-search/store/state';
    import { Mutations } from '../../../keyword-search/store/mutations'
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

            },
            ...props
        },

        setup(p) {
            const store = useStore();

            const ispopup = ref(p.popup as boolean);
           
            console.log("inside popup New: " + JSON.stringify(p.popup));
            return {
                isPopupVisible: computed(() => (store.state as State).popupVisibility),
                setPopupVisibility: (visibility: boolean) => store.commit(Mutations.SET_POPUP_VISIBILITY, visibility),

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
    <div class="popup" >
        <div class="popup-inner">
            <button class="popup-close" @click="setPopupVisibility(!isPopupVisible)">
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