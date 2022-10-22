<script lang="ts" setup>

import {useRoute, useRouter} from 'vue-router'
import {Guid} from 'guid-typescript'
import { computed } from 'vue';

const route = useRoute()
const id = computed(() => route.params?.id as unknown as Guid)

</script>

<template>
    <h4>Delete <slot name="object-type"/></h4>
    <slot name="delete-delegate"></slot>
    {{id}}
</template>