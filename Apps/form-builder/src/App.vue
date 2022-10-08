<script setup lang="ts">
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'

import { getActivePinia } from 'pinia'
import { Guid } from 'guid-typescript'
import {onMounted, ref} from 'vue'

import {FormBuilder} from 'applets'

const piniaInstance = getActivePinia();
const root = ref(null as HTMLElement | null)

onMounted(() => {
    const val = root.value?.parentElement;

    console.log('Mounted ...', val)
})
</script>

<template>
    <span ref="root"/>
    <FormBuilder :pinia-instance="piniaInstance" repository-root="https://localhost:5020/" />
</template>

