<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { ref, computed } from "vue"
    import { useEntityEditorStore } from "../store"
    import { FormEntry } from '../../shared/form-models'
    import { EntityTemplate } from '../../entity-template-builder/models'
    import { default as Form } from '../../form-submission/components/Form.vue'
import { EntityData } from '../models'

    const store = useEntityEditorStore();
    const props = defineProps<{
        formEntries: FormEntry[],
        entity: EntityData
    }>();
    const selectedFormId = ref(props.formEntries[0]?.formId)
    const selectedForm = computed(() => store.entityTemplate?.forms?.find(form => form.id === selectedFormId.value))
</script>

<template>
    <div v-if="formEntries">
        <span v-for="formEntry in formEntries" :key="formEntry.id"><a href="#" @click="selectedFormId=formEntry.formId">{{formEntry.name}} | </a></span>
        <Form v-if="selectedForm" :model="selectedForm" :entity="entity"></Form>
    </div>
    
</template>
