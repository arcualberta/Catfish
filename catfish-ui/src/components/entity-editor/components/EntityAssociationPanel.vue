<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { computed } from "vue"
    import { eEntityType } from "../../shared/constants"
    import { useEntityEditorStore } from "../store"
    import { EntityData } from '../models'

    const store = useEntityEditorStore();
    const props = defineProps<{
        entity: EntityData,
        relationshipType: string,
        panelTitle:string
    }>();
    const dataList = computed(() => props.entity?.subjectRelationships.filter(form => form.name == props.relationshipType))
    
    
</script>

<template>
    <b-row>
        <b-col class="col-sm-5">
            <h6>{{relationshipType}}</h6>
        </b-col>
        <b-col class="col-sm-2">

        </b-col>
        <b-col class="col-sm-5">
            <h6>{{panelTitle}}</h6>
        </b-col>
    </b-row>
    <b-row>
        <b-col class="col-sm-5">
            <div class="form-field-border">
                
            </div>
        </b-col>
        <b-col class="col-sm-2">
            <b-row>
                <b-col class="col-sm-4">

                </b-col>
                <b-col class="col-sm-4">
                    <button class="btn btn-primary"><font-awesome-icon icon="fa-solid fa-arrow-left" /></button>
                </b-col>
                <b-col class="col-sm-4">

                </b-col>
            </b-row>
        </b-col>
        <b-col class="col-sm-5">
            <div class="form-field-border">
            </div>
        </b-col>
    </b-row>


    <div class="row">
        <v-multiselect-listbox :options="['Alabama', 'California']"></v-multiselect-listbox>
    </div>

</template>