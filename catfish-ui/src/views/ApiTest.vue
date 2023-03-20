<script lang="ts" setup>
import {EntityTemplateProxy} from "@/api/entityTemplateProxy"
import {CollectionProxy} from "@/api/collectionProxy"
import {ItemProxy} from "@/api/itemProxy"
import {FormProxy} from "@/api/formProxy" //formTemplateProxy
import {WorkflowProxy} from "@/api/workflowProxy"
import {FormDataProxy} from "@/api/formDataProxy"

import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { EntityEntry, FormEntry } from "@/components/shared/models/listEntries";
import { Guid } from "guid-typescript";
import { ref, watch } from "vue";
import { eEntityType, eState } from "@/components/shared/constants"
import { useEntityEditorStore } from '../components/entity-editor/store';
import { FormTemplate, FormData } from "@/components/shared/form-models"
import { Workflow } from "@/components/workflow-builder/models"
import { createGuid } from "@/components/shared/form-helpers"

const entityEditorStore = useEntityEditorStore();
const error = ref("")

const entityTemplates = ref([] as TemplateEntry[])
EntityTemplateProxy.List().then(val =>  entityTemplates.value = val).catch(e => error.value = e);

const entityTemplate = ref(null as null | EntityTemplate)
watch(() => entityTemplates.value, async newList => {
    if(newList.length > 0)
        EntityTemplateProxy.Get((newList[0] as TemplateEntry).id).then(val =>  entityTemplate.value = val).catch(e => error.value = e);
    })

const newEntityTemplate = ref({ name: "Test Entity Template", description: "This is a test entity template." } as EntityTemplate)
EntityTemplateProxy.Post(newEntityTemplate.value as EntityTemplate).catch(e => error.value = e)

//Displays a deep clone for update since we don't want the newEntityTemplate to be changed in this display
//when we test the PUT call. However, we will actually change this entry in the database. This is okay because
//we will be deleting it from the database in the DELETE test anyway.
const updatedEntityTemplate = ref(JSON.parse(JSON.stringify(newEntityTemplate.value)) as EntityTemplate);
updatedEntityTemplate.value.name = "Updated Entity Template"
EntityTemplateProxy.Put(updatedEntityTemplate.value as EntityTemplate).then(val => {}).catch(e => error.value = "Updated Entity Template: " + e)

//==============================   FORM
const formTemplates = ref([] as FormEntry[])
FormProxy.List().then(val =>  formTemplates.value = val).catch(e => error.value = e);

const aformTemplate = ref(null as null | FormTemplate)
watch(() => formTemplates.value, async newList => {
    if(newList.length > 0)
        FormProxy.Get((newList[0] as FormEntry).id).then(val =>  aformTemplate.value = val).catch(e => error.value = e);
    })

const newFormTemplate = ref({ name: "Test Form Template", description: "This is a test form template." } as FormTemplate)
FormProxy.Post(newFormTemplate.value as FormTemplate).catch(e => error.value = e)

//Displays a deep clone for update since we don't want the newEntityTemplate to be changed in this display
//when we test the PUT call. However, we will actually change this entry in the database. This is okay because
//we will be deleting it from the database in the DELETE test anyway.
const updatedFormTemplate = ref(JSON.parse(JSON.stringify(newFormTemplate.value)) as FormTemplate);
updatedFormTemplate.value.name = "Updated Form Template"
FormProxy.Put(updatedFormTemplate.value as FormTemplate).then(val => {}).catch(e => error.value = "Updated Form Template: " + e)

//==============================   WORKFLOW
const wkflows = ref([] as Workflow[])
WorkflowProxy.List().then(val =>  wkflows.value = val).catch(e => error.value = e);

const awkflow = ref(null as null | Workflow)
watch(() => wkflows.value, async newList => {
    if(newList.length > 0)
        WorkflowProxy.Get((newList[0] as Workflow).id).then(val =>  awkflow.value = val).catch(e => error.value = e);
    })

const newWkFlow = ref({id:createGuid(), name: "Test Workflow Template", description: "This is a test workflow template." } as FormTemplate)
WorkflowProxy.Post(newWkFlow.value! as Workflow).catch(e => error.value = e)

//Displays a deep clone for update since we don't want the newEntityTemplate to be changed in this display
//when we test the PUT call. However, we will actually change this entry in the database. This is okay because
//we will be deleting it from the database in the DELETE test anyway.
const updatedWorkflow = ref(JSON.parse(JSON.stringify(newWkFlow.value)) as Workflow);
updatedWorkflow.value.name = "Updated Workflow Template"
WorkflowProxy.Put(updatedWorkflow.value as Workflow).then(val => {}).catch(e => error.value = "Updated workflow Template: " + e)


//===============================Collections
const collections = ref([] as EntityEntry[])
CollectionProxy.List().then(val =>  collections.value = val).catch(e => error.value = e);
// acollection details
const acollection = ref(null as null | EntityData)
watch(() => collections.value, async newList => {
    if(newList.length > 0)
        CollectionProxy.Get((newList[0] as EntityEntry).id).then(val =>  acollection.value = val).catch(e => error.value = e);
    })

/*const newCollection = ref(null as null | EntityData);
newCollection.value = entityEditorStore.createNewEntity(eEntityType.Collection) as unknown as EntityData;
CollectionProxy.Post(newCollection.value as EntityData).catch(e => error.value = e)



const updatedCollection = ref(JSON.parse(JSON.stringify(acollection.value)) as EntityData);
updatedCollection.value!.updated= new Date();
CollectionProxy.Put(updatedCollection.value as EntityData).then(val => {}).catch(e => error.value = "Updated Collection: " + e)
*/

//=============================== Items
const items = ref([] as EntityEntry[])
ItemProxy.List().then(val =>  items.value = val).catch(e => error.value = e);
// acollection details
const anItem = ref(null as null | EntityData)
watch(() => items.value, async newList => {
    if(newList.length > 0)
        ItemProxy.Get((newList[0] as EntityEntry).id).then(val =>  anItem.value = val).catch(e => error.value = e);
    })

/*const newCollection = ref(null as null | EntityData);
newCollection.value = entityEditorStore.createNewEntity(eEntityType.Collection) as unknown as EntityData;
CollectionProxy.Post(newCollection.value as EntityData).catch(e => error.value = e)



const updatedCollection = ref(JSON.parse(JSON.stringify(acollection.value)) as EntityData);
updatedCollection.value!.updated= new Date();
CollectionProxy.Put(updatedCollection.value as EntityData).then(val => {}).catch(e => error.value = "Updated Collection: " + e)
*/


//=============================== FormSubmissions
const formSubmissions = ref([] as Guid[])
FormDataProxy.List().then(val =>  formSubmissions.value = val).catch(e => error.value = e);
// acollection details
const aFormData = ref(null as null | FormData)
watch(() => formSubmissions.value, async newList => {
    if(newList.length > 0)
        FormDataProxy.Get(newList[0] as Guid).then(val =>  aFormData.value = val).catch(e => error.value = e);
    })
</script>

<template>

    <div v-if="error" class="alert alert-danger mt-2">{{ error }}</div>

    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Entity Template List</h4>
        {{ entityTemplates }}
    </div>

    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Entity Template</h4>
        {{ entityTemplate }}
    </div>

    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>New Entity Template</h4>   
        {{ JSON.parse(JSON.stringify(newEntityTemplate)) }}
    </div>

    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Updated Entity Template</h4>   
        {{ JSON.parse(JSON.stringify(updatedEntityTemplate)) }}
    </div>
    <!--                    F O R M S               -->
    <div class="alert alert-success mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Form Listing</h4>   
        {{ JSON.stringify(formTemplates) }}
    </div>
     <div class="alert alert-success mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>A Form </h4>   
        {{ JSON.stringify(aformTemplate) }}
    </div>
     <div class="alert alert-success mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4> New Form </h4>   
        {{ JSON.stringify(newFormTemplate) }}
    </div>
    <div class="alert alert-success mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4> Updated Form </h4>   
        {{ JSON.stringify(updatedFormTemplate) }}
    </div>
     <!--                    W O R K F L O W S               -->

     <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Workflows List</h4>
        {{ JSON.stringify(wkflows) }}
    </div>
    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>A Workflow</h4>
        {{ JSON.stringify(awkflow) }}
    </div>
    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>New Workflow</h4>
        {{ JSON.stringify(newWkFlow) }}
    </div>
    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Updated Workflow</h4>
        {{ JSON.stringify(updatedWorkflow) }}
    </div>

     <!--                    C O L L E C T I O N S               -->
    <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Collections List</h4>
        {{ collections }}
    </div>
    <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>A Collection</h4>
        {{ acollection }}
    </div>
    <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Updated Collection</h4>
        {{ updatedCollection }}
    </div>
 <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>New Collection</h4>
        {{ newCollection }}
    </div>

    <!--                    I T E M S               -->
    <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Items List</h4>
        {{ items }}
    </div>
    <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>An Item</h4>
        {{ anItem }}
    </div>
   <!-- <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>Updated Collection</h4>
        {{ updatedCollection }}
    </div>
 <div class="alert alert-warning mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>New Collection</h4>
        {{ newCollection }}
    </div>
    -->


     <!--                    F O R M  S U B M I S S I O N S               -->
     <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>FormSubmission List</h4>
        {{ JSON.stringify(formSubmissions) }}
    </div>
    <div class="alert alert-info mt-2" style="max-height: 200px; overflow-y: scroll;">
        <h4>A Form Submission Data</h4>
        {{ JSON.stringify(aFormData) }}
    </div>
</template>
