<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import { SolrSearchPanel, useSolrSearchStore } from '../components'
    import { default as config, eShowtimeEntryType, solrFields } from "@/appsettings";
    import { SolrEntryType } from '@/components/solr-search-panel/models';

    const route = useRoute()
    const entityId = route.params.id as unknown as Guid
    
     const apiRoot= config.dataRepositoryApiRoot + "/api/solr";

     const store = useSolrSearchStore();
     store.searchFieldDefinitions = solrFields;


    const resultFieldNames = [] as string[];
    const entryTypeFieldName = "entry_type_s"; 
    const entryTypeFieldOptions = [
        {name:"raw-movie", label:"Movies", entryType: eShowtimeEntryType.Movie}, 
        {name:"raw-theater", label:"Theaters", entryType: eShowtimeEntryType.Theater}, 
        {name:"raw-showtime", label:"Showtimes", entryType: eShowtimeEntryType.Showtime}
    ] as SolrEntryType[];

</script>

<template>
    <h5>Solr Search </h5>
    <SolrSearchPanel :result-field-names="resultFieldNames" :entry-type-field-name="entryTypeFieldName" :entry-type-field-options="entryTypeFieldOptions" />
</template>


