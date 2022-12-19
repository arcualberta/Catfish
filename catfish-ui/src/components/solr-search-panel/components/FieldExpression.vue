<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { SearchFieldDefinition } from '../models'
    import { computed, ref } from 'vue';
    import { eFieldType, eFieldConstraint } from '../../shared/constants'
    import { getFieldConstraintLabel, eFieldConstraintValues } from '@/components/shared/constants'
    import { default as FieldConstraint } from './FieldConstraint.vue'
    import { default as ConfirmPopUp } from '../../../components/shared/components/pop-up/ConfirmPopUp.vue';
    /* import the fontawesome core */
    import { library } from '@fortawesome/fontawesome-svg-core'

    /* import font awesome icon component */
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

    /* import specific icons */
    import * as faIcons from '@fortawesome/free-solid-svg-icons'
    /* add icons to the library */
    library.add(faIcons.faCircleCheck)
    library.add(faIcons.faCircleXmark)
    library.add(faIcons.faPenToSquare)
    library.add(faIcons.faCirclePlus)
    library.add(faIcons.faQuestionCircle)
    library.add(faIcons.faThList)
    library.add(faIcons.faArrowLeft)

    const props = defineProps<{
        searchFields: SearchFieldDefinition[],
        value: string[] 
    }>();

    const popupTrigger = ref(false);
    const field = ref(null as null | SearchFieldDefinition );
    const readOnly = computed(() => field.value && field.value.type >  0);
    const togglePopup = () => (popupTrigger.value = !popupTrigger.value);
    const createConstraint = () => {
        console.log("Create a constraint")
        popupTrigger.value = !popupTrigger.value
    };
    const createExpression = () => {
        console.log("Create a expression")
        popupTrigger.value = !popupTrigger.value
    };
    </script>

<template>
    Field Expression <br />
    <div style="border-style: solid;border-color: red;">
        <FieldConstraint :searchFields="props.searchFields" :value="props.value"></FieldConstraint>
        <div style="float:right">
            <font-awesome-icon icon="fa-solid fa-circle-plus" @click="togglePopup()" class="fa-icon plus" />
            <ConfirmPopUp v-if="popupTrigger" >
            <template v-slot:header>
                Add a Search criteria.
                <button type="button"
                        class="btn-close"
                        @click="popupTrigger=false">
                    x
                </button>
            </template>
            <template v-slot:body>
                
            </template>
            <template v-slot:footer>
                <button type="button"
                        class="modal-confirm-btn"
                        @click="createConstraint"
                        aria-label="Close modal">
                    Constraint
                </button>
                
                <button type="button"
                        class="modal-cancel-btn"
                        style="margin-left:10px"
                        @click="createExpression"
                        aria-label="Close modal">
                    Expression
                </button>
            </template>
        </ConfirmPopUp>
        </div>
    </div>
</template>