<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { SearchFieldDefinition } from '../models'
    import { computed, ref } from 'vue';
    import { eFieldType } from '../../shared/constants'

    const props = defineProps<{
        searchFields: SearchFieldDefinition[]
    }>();

    const type = ref(eFieldType.Text);
    const readOnly = ref(false);
    </script>
<template>
    Field Selector
    <b-row>
        <b-col class="col-sm-4">

        </b-col>
        <b-col class="col-sm-4">
           <input type=text  readonly="readonly"/>
        </b-col>
        <b-col class="col-sm-4">
            <select class="form-select">
                <option v-for="opt in searchFields">{{opt.label}}</option>
            </select>
        </b-col>
    </b-row>

</template>