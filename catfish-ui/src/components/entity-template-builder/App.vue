<template>
    <h3>Entity Template Builder</h3>
    <button @click="createTemplate">New Template</button>
    <button @click="saveTemplate">Save</button>

    <div v-if="template">
        <div>Name : <input v-model="template.name" /> </div>
        <div>Description : <textarea v-model="template.description" /> </div>
        <div>State: {{template.state}}</div>
        <div>
            <h5>Metadata Forms</h5>
            <div v-for="frm in template.entityTemplateSettings.metadataForms" :key="frm.formId">
                <FormEntryTemplate :model="frm" />
            </div>
            <button @click="addMetadataForm">+ Add</button>
        </div>
        <div>
            <h5>Data Forms</h5>
            <div v-for="frm in template.entityTemplateSettings.dataForms" :key="frm.formId">
                <FormEntryTemplate :model="frm" />
            </div>
            <button @click="addDataForm">+ Add</button>
        </div>

        <div class="alert alert-info">{{JSON.stringify(template)}}</div>
    </div>

</template>

<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed, onMounted } from 'vue'
    import { useEntityTemplateBuilderStore } from './store';
    import { AppletAttribute } from '@/components/shared/props'
    import { default as FormEntryTemplate } from './components/FormEntry.vue';
    import { Guid } from 'guid-typescript';
    import { FormEntry } from './models';
        import { useRouter} from 'vue-router';

    const props = defineProps<{
        dataAttributes?: AppletAttribute | null,
        queryParameters?: AppletAttribute | null,
        piniaInstance: Pinia,
        
    }>();

    const store = useEntityTemplateBuilderStore(props.piniaInstance);
    const createTemplate = () => store.newTemplate();
    const template = computed(() => store.template);
    const router = useRouter();
    const addMetadataForm = () => {
        store.template?.entityTemplateSettings.metadataForms?.push({ formId: Guid.createEmpty(), name: "" } as FormEntry);
    }

    const addDataForm = () => {
        store.template?.entityTemplateSettings.dataForms?.push({ formId: Guid.createEmpty(), name: "" } as FormEntry);
    }

    const saveTemplate = ()=>store.saveTemplate();

    onMounted(() => {
        store.loadForms();
       if(template.value){ 
            if(template.value.id?.toString() !== Guid.EMPTY)
               router.push(`/edit-entity-template/${template.value.id}`)
       }
    });



</script>
<style scoped src="./style.css"></style>
