<template>
    <div class="alert alert-success">
        <div>Name : <input v-model="model.name" /> </div>
        <label :for="model.formId.toString()">Form:</label>

        <select v-model="model.formId" :name="model.formId.toString()">
            <option v-for="entry in store.formEntries" :key="entry.formId.toString()" :value="entry.formId">{{entry.name}}</option>
        </select>

        <div>FormId:  {{model.formId}}</div>
    </div>
</template>

<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { FormEntry } from '../models'
    import { useEntityTemplateBuilderStore } from '../store';

    const props = defineProps<{
        model: FormEntry
    }>();

    const store = useEntityTemplateBuilderStore();

</script>
<style scoped src="../style.css"></style>
