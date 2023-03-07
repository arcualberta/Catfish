<script lang="ts" setup>
import {EntityTemplateProxy} from "@/api/entityTemplateProxy"
import { TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { Guid } from "guid-typescript";
import { ref, watch } from "vue";

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

</template>
