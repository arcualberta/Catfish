<script setup lang="ts">
    import { Pinia } from 'pinia';
    import { computed } from 'vue';
    import { FormEntry } from '../../shared/form-models';
    import { useEntityTemplateBuilderStore } from '../store';
    import { SelectableOption } from '@/components/shared/components/form-field-selection-dropdown/models'
    const props = defineProps<{
        model: FormEntry
    }>();

    const store = useEntityTemplateBuilderStore();

</script>

<template>
    <b-row>
        <b-col class="col-sm-11">
            <b-row>
                <b-col class="col-sm-2">
                    <h6>Name :</h6>
                </b-col>
                <b-col class="col-sm-10">
                    <b-form-input v-model="model.name"></b-form-input>
                </b-col>
            </b-row>
            <br />
            <b-row>
                <b-col class="col-sm-2">
                    <label :for="model.formId.toString()">Form:</label>
                </b-col>
                <b-col class="col-sm-10">
                    <select v-model="model.formId" :name="model.formId.toString()" class="form-select">
                        <option v-for="entry in store.formEntries" :key="entry.formId.toString()" :value="entry.formId">{{entry.name}}</option>
                    </select>
                </b-col>
            </b-row>
        </b-col>
        <b-col class="col-sm-1">
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="store.deleteFormEntry(model.id)" class="fa-icon delete" />
        </b-col>
    </b-row>
    {{model.formId}}
</template>

<style scoped src="../style.css"></style>
