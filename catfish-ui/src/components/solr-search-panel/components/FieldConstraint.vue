<script setup lang="ts">
import { Pinia } from 'pinia'
import { SearchFieldDefinition } from '../models'
import { computed, ref } from 'vue';
import { eFieldType, eFieldConstraint } from '../../shared/constants'
import { getFieldConstraintLabel, eFieldConstraintValues } from '@/components/shared/constants'
import { BRow, BCol, BFormInput } from 'bootstrap-vue-3';
import { FieldConstraint } from '../models/FieldConstraint';
import { useSolrSearchStore } from '../store';
import { validate } from 'uuid';

const props = defineProps<{
    model: FieldConstraint,
    searchFields?: SearchFieldDefinition[]
}>();

const field = ref(null as null | SearchFieldDefinition ) ;
//const readOnly = computed(() => field.value && field.value.type >  0);

const store = useSolrSearchStore();
const searchFields = computed(() => store.searchFieldDefinitions)

const txtValue = computed({
        get: () => props.model.value as unknown as string,
        set: (val) => {
            props.model.value = val as unknown as object;
        }
    })

 const fieldValues = computed(() => JSON.parse(JSON.stringify(props.model.field)) )  
    const selectedOption = computed({
        get: () => fieldData?.value?.selectedOptionIds && fieldData.value.selectedOptionIds.length > 0 ? fieldData.value.selectedOptionIds[0] as unknown as string : Guid.EMPTY as unknown as string ,
        set: (opt) => props.model.value = [opt as unknown as string]
    })
</script>
<template>
    <b-row>
        <b-col class="col-sm-5">
            <select class="form-select" v-model="model.field">
                <option v-for="opt in searchFields" :value="opt">{{opt.label}}</option>
            </select>
        </b-col>
        <b-col class="col-sm-2">
            <select class="form-select" v-model="model.constraint">
                <option v-for="con in eFieldConstraintValues" :value="con">{{getFieldConstraintLabel(con)}}</option>
            </select>
        </b-col>
        <b-col class="col-sm-5">
            <div v-if="fieldValues.type === eFieldType.Text">
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldValues.type === eFieldType.Date">
                <b-form-input type="date" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldValues.type === eFieldType.Integer">
                <b-form-input type="number" step='1' v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldValues.type === eFieldType.Decimal">
                <b-form-input type="number" :step='Math.pow(10, 2)' v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldValues.type === eFieldType.Email">
                <b-form-input type="email" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldValues.type === eFieldType.Radio">
                <span v-for="opt in fieldValues.options">
                    <input type="radio" name="opt" :value="(opt as unknown as string)"/>{{opt}}
                </span>
            </div>
            <div v-else>
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>


        </b-col>

    </b-row>

</template>