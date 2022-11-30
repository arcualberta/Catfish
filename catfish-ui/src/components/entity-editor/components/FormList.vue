<script setup lang="ts">
import { ref, computed } from "vue"
import { useEntityEditorStore } from "../store"
import { FormEntry } from '../../shared'
import { default as Form } from '../../form-submission/components/Form.vue'
import { EntityData } from '../models'
import { Guid } from "guid-typescript"

const store = useEntityEditorStore();
const props = defineProps<{
    formEntries: FormEntry[],
    entity: EntityData
}>();
const selectedFormId = ref(props.formEntries[0]?.id.toString())
const selectedForm = computed(() => store.entityTemplate?.forms?.find(form => form.id.toString() === selectedFormId.value))

</script>

<template>
    <div v-if="formEntries">
        <span v-for="formEntry in formEntries" :key="formEntry.id.toString()"><a href="#" @click="selectedFormId=formEntry.id">{{formEntry.name}} | </a></span>
        <Form v-if="selectedForm" :model="selectedForm" :entity="entity"></Form>
    </div>    
</template>
