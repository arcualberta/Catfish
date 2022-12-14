<script setup lang="ts">
    import { default as FieldConstraint } from './components/FieldConstraint.vue'
    import { SolrFields } from './appsettings';
    </script>
<template>
    Solr search
    <FieldConstraint :searchFields="SolrFields"></FieldConstraint>

</template>

