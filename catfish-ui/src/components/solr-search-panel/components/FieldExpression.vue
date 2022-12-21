<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { SearchFieldDefinition } from '../models'
    import { computed, ref } from 'vue';
    import { eFieldType, eFieldConstraint } from '../../shared/constants'
    import { getFieldConstraintLabel, eFieldConstraintValues, eConstraintType ,eOperatorValues, getOperatorLabel} from '@/components/shared/constants'
    import { default as FieldExpressionTemplate } from './FieldExpression.vue'
    import { default as FieldConstraintTemplate } from './FieldConstraint.vue'
    import { default as ConfirmPopUp } from '../../../components/shared/components/pop-up/ConfirmPopUp.vue';
    /* import the fontawesome core */
    import { library } from '@fortawesome/fontawesome-svg-core'

    /* import font awesome icon component */
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

    /* import specific icons */
    import * as faIcons from '@fortawesome/free-solid-svg-icons'
import { ConstraintType, FieldExpression } from '../models/FieldExpression';
import { FieldConstraint } from '../models/FieldConstraint';

    /* add icons to the library */
    library.add(faIcons.faCircleCheck)
    library.add(faIcons.faCircleXmark)
    library.add(faIcons.faPenToSquare)
    library.add(faIcons.faCirclePlus)
    library.add(faIcons.faQuestionCircle)
    library.add(faIcons.faThList)
    library.add(faIcons.faArrowLeft)

    const props = defineProps<{
        model: FieldExpression
    }>();

    const popupTrigger = ref(false);
    const field = ref(null as null | SearchFieldDefinition );
    const readOnly = computed(() => field.value && field.value.type >  0);
    const togglePopup = () => (popupTrigger.value = !popupTrigger.value);

    const createConstraint = () => {
        addComponent(new FieldConstraint())        
        popupTrigger.value = !popupTrigger.value
    };

    const createExpression = () => {
        addComponent(new FieldExpression())
        popupTrigger.value = !popupTrigger.value
    };

    const addComponent = (component: ConstraintType) =>{
        if(!props.model.expressionComponents || !props.model.operators){
            props.model.expressionComponents = [];
            props.model.operators = [];
        }

        props.model.expressionComponents.push(component);
        if(props.model.expressionComponents.length > 1)
        props.model.operators.push(eFieldConstraint.Equals);
    }

    </script>

<template>
    <div class="form-field-border row">
        <div class="col-md-1"></div>
        <div v-if="model.expressionComponents?.length > 0" class="col-md-11" >
            <FieldExpressionTemplate v-if="model.expressionComponents[0].getType() === eConstraintType.FieldExpression" :model="(model.expressionComponents[0] as unknown as FieldExpression)" /> 
            <FieldConstraintTemplate v-if="model.expressionComponents[0].getType() === eConstraintType.FieldConstraing" :model="model.expressionComponents[0] as unknown as FieldConstraint" /> X
        </div>

        <div v-for="(op, index) in model.operators" class="row">
            <div class="col-md-12">
                <div class="col-md-1">
                    <select class="form-select" v-model="model.operators[index]">
                        <option v-for="operator in eOperatorValues" :value="operator">{{getOperatorLabel(operator)}}</option>
                    </select>
                </div>
                
            </div>
            <div class="col-md-1"></div>
            <div class="col-md-11" >
                <FieldExpressionTemplate v-if="model.expressionComponents[index+1].getType() === eConstraintType.FieldExpression" :model="(model.expressionComponents[index+1] as unknown as FieldExpression)" />
                <FieldConstraintTemplate v-if="model.expressionComponents[index+1].getType() === eConstraintType.FieldConstraing" :model="(model.expressionComponents[index+1] as FieldConstraint)" /> X
            </div>
        </div>

        <div style="text-align:right">
            <font-awesome-icon icon="fa-solid fa-circle-plus" @click="togglePopup()" class="fa-icon plus" @class="m-2"/>
            <ConfirmPopUp v-if="popupTrigger" >
                <template v-slot:header>
                    Add a search criteria.
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
<style scoped>
   .form-field-border {
    border: 1px solid;
    padding: 30px 16px;
    border-radius: 16px;
    min-height: 100px;
    margin-bottom: 10px;
}
.btn-close {
    position: absolute;
    top: 0;
    right: 0;
    border: none;
    font-size: 20px;
    padding: 10px;
    cursor: pointer;
    font-weight: bold;
    color: #db2424;
    background: transparent;
}
</style>