<script setup lang="ts">import { computed } from 'vue';
import { SearchResult } from '../models';
import { useSolrSearchStore } from '../store';

    const props = defineProps<{
        model: SearchResult
    }>();

    const store = useSolrSearchStore()

    const first = computed(() => props.model.offset + 1)
    const last = computed(() => props.model.offset + props.model.resultEntries.length)
    const hasPrev = computed(() => first.value > 1)
    const hasNext = computed(() => last.value < props.model.totalMatches)

</script>

<template>
    <div class="mt-2">
        <span v-if="hasPrev" class="link" @click="store.previous()">&lt;&lt;&lt;</span>
        {{ first }} to {{ last }} of {{ model.totalMatches }}
        <span v-if="hasNext" class="link" @click="store.next()">&gt;&gt;&gt;</span></div>
    <hr />
    <table>
        <tbody>
            <tr v-for="row in model.resultEntries">
                <td>
                    {{ JSON.stringify(row) }}
                </td>
            </tr>
            </tbody>
    </table>
</template>

<style scoped>
.link{
    color: #2626ea;
}
.link:hover{
    cursor: pointer;
}
</style>

