<script setup lang="ts">

const props = defineProps < { 
    tabs: Array<string>
} > ();
</script>

<template>
    <div>
        <b-tabs > content-class="mt-3">
            <b-tab v-for="tab in tabs" :key="tab" :title="tab"><slot :name="tab"></slot></b-tab>
        </b-tabs>
    </div>
</template>