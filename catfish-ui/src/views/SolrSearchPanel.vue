<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
   // import { useSolrSearchStore } from '../components/solr-search-panel/store';
    import { SolrSearchPanel, useSolrSearchStore } from '../components'
    import { default as config, solrFields } from "@/appsettings";

    const route = useRoute()
    const entityId = route.params.id as unknown as Guid
    
     const apiRoot= config.dataRepositoryApiRoot + "/api/solr";

     const store = useSolrSearchStore();
     store.searchFieldDefinitions = solrFields;

</script>

<template>
    <h5>Solr Search </h5>
    <SolrSearchPanel     />
</template>


