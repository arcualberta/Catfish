<template>
    <h3>Entity Template Builder</h3>
</template>

<script setup lang="ts">
import { Pinia } from 'pinia'
import { onMounted } from 'vue'
import config from '../../appsettings'


const props = defineProps < {
        piniaInstance: Pinia,
        
     } > ();

</script>
<style scoped src="./style.css"></style>
