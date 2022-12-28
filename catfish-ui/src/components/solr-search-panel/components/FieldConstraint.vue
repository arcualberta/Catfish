<script setup lang="ts">
import { SearchFieldDefinition } from '../models'
import { computed, ref } from 'vue';
import { eFieldType, getFieldConstraintToolTip } from '../../shared/constants'
import { getFieldConstraintLabel, eFieldConstraintValues } from '@/components/shared/constants'
import { BRow, BCol, BFormInput } from 'bootstrap-vue-3';
import { FieldConstraint } from '../models/FieldConstraint';
import { useSolrSearchStore } from '../store';
import { Guid } from 'guid-typescript';

    const props = defineProps<{
        model: FieldConstraint,
        searchFields?: SearchFieldDefinition[]
    }>();

    const field = ref(null as null | SearchFieldDefinition ) ;
    //const readOnly = computed(() => field.value && field.value.type >  0);
    const fieldName = ref(`opt-${Guid.create() as unknown as string}`);

    const store = useSolrSearchStore();
    const activeFieldList = computed(() => store.activeFieldList)
/*    
    const searchFields = computed(() => {
        if(store.selectedEntryType){
            return store.searchFieldDefinitions?.filter(fd => Array.isArray(fd.entryType) 
                ? (fd.entryType as number[]).includes(store.selectedEntryType!.entryType) 
                : (fd.entryType as number) === store.selectedEntryType!.entryType);
        }
        else{
            return store.searchFieldDefinitions;
        }
    })
*/
    const txtValue = computed({
        get: () => props.model.value as unknown as string,
        set: (val) => {
            props.model.value = val as unknown as object;
        }
    })

    const numValue = computed({
        get: () => props.model.value as unknown as number,
        set: (val) => {
            props.model.value = Number(val) as unknown as object;
        }
    })

    const fieldType = computed(() => props.model.field?.type)

    const tooltip = computed(() => getFieldConstraintToolTip(props.model.constraint!))

</script>
<template>
    <b-row>
        <b-col class="col-sm-5">
            <select class="form-select" v-model="model.field">
                <option v-for="opt in activeFieldList" :value="opt">{{opt.label}}</option>
            </select>
        </b-col>
        <b-col class="col-sm-2">
            <select class="form-select" v-model="model.constraint" v-b-tooltip.hover :title="tooltip">
                <option v-for="con in eFieldConstraintValues" :value="con">{{getFieldConstraintLabel(con)}}</option>
            </select>
        </b-col>
        <b-col class="col-sm-5">
            <div v-if="fieldType === eFieldType.Text">
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldType === eFieldType.Date">
                <b-form-input type="date" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldType === eFieldType.Integer">
                <b-form-input type="number" step='1' v-model="numValue"></b-form-input>
            </div>
            <div v-else-if="fieldType === eFieldType.Decimal">
                <b-form-input type="number" :step='Math.pow(10, 2)' v-model="numValue"></b-form-input>
            </div>
            <div v-else-if="fieldType === eFieldType.Email">
                <b-form-input type="email" v-model="txtValue"></b-form-input>
            </div>
            <div v-else-if="fieldType === eFieldType.Radio">
                <span v-for="opt in model.field!.options">
                    <input type="radio" :name="fieldName" v-model="txtValue" :value="(opt as unknown as string)"/>{{opt}}
                </span>
            </div>
            <div v-else>
                <b-form-input type="text" v-model="txtValue"></b-form-input>
            </div>

        </b-col>

    </b-row>
</template>

<style scoped>
.form-select, input.form-control{
    font-size: xx-small;
}
</style>