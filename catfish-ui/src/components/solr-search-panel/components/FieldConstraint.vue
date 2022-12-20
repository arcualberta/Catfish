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
            <div v-if="field?.type === eFieldType.Text">
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="field?.type === 2">
                <b-form-input type="date"></b-form-input>
            </div>
            <div v-else-if="field?.type === 3">
                <b-form-input type="number" step='1'></b-form-input>
            </div>
            <div v-else-if="field?.type === 4">
                <b-form-input type="number" :step='Math.pow(10, 2)'></b-form-input>
            </div>
            <div v-else-if="field?.type === 5">
                <b-form-input type="email"></b-form-input>
            </div>
            <div v-else-if="field?.type === 8">
                <span v-for="opt in field.options">
                    <input type="radio" />{{opt}}
                </span>
            </div>
            <div v-else>
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>


        </b-col>

    </b-row>

</template>