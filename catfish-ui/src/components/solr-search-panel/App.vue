<script setup lang="ts">
    import { default as FieldSelector } from './components/FieldSelector.vue'
    import { SolrFields } from './appsettings';
    </script>
<template>
    Solr search
    <FieldSelector :searchFields="SolrFields"></FieldSelector>

</template>

