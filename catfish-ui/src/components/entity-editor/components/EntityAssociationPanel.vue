<script setup lang="ts">
    import { Pinia, storeToRefs } from 'pinia'
    import { computed } from "vue"
    import { eEntityType } from "../../shared/constants"
    import { useEntityEditorStore } from "../store"
    import { EntityData } from '../models'
    import { EntitySelectionList } from "../../shared/components/"
    import { Guid } from 'guid-typescript'

    const store = useEntityEditorStore();
    const props = defineProps<{
        entity: EntityData,
        relationshipType: string,
        panelTitle:string
    }>();

    const {entity, storeId} = storeToRefs(store);
    const dataList = computed(() => props.entity?.subjectRelationships.filter(form => form.name == props.relationshipType))
    
    const addRelationship= ()=>{store.AddToRelationObject()};
    
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
                <div v-if="entity.subjectRelationships">
                  <div v-for="rel in entity.subjectRelationships" :key="rel.subjectEntityId">
                    {{rel.subjectEntityId}}
                  </div>
                </div>
            </div>
        </b-col>
        <b-col class="col-sm-2">
            <b-row>
                <b-col class="col-sm-4">

                </b-col>
                <b-col class="col-sm-4">
                    <button class="btn btn-primary" @click="addRelationship"><font-awesome-icon icon="fa-solid fa-arrow-left" /></button>
                </b-col>
                <b-col class="col-sm-4">

                </b-col>
            </b-row>
        </b-col>
        <b-col class="col-sm-5">
            <div class="form-field-border">
                <EntitySelectionList :storeId="storeId.toString()" :entityType="eEntityType.Item"></EntitySelectionList>
            </div>
        </b-col>
    </b-row>

</template>