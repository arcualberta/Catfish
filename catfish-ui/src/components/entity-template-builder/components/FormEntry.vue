<template>
    <h3>Form Entry</h3>
    <div>Name: {{model.name}}</div>
    <div>FormId:  {{model.formId.toString()}}</div>
</template>

<script setup lang="ts">

import {FormEntry} from '../models'


const props = defineProps < {    
        model: FormEntry  
     } > ();

</script>
<style scoped src="../style.css"></style>
