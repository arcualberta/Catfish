<script setup lang="ts">
    import { Pinia } from 'pinia'
    import { SearchFieldDefinition } from '../models'
    import { computed, ref } from 'vue';
    import { eFieldType, eFieldConstraint } from '../../shared/constants'
    import { getFieldConstraintLabel, eFieldConstraintValues } from '@/components/shared/constants'
    import { default as FieldConstraint } from './FieldConstraint.vue'
    const props = defineProps<{
        searchFields: SearchFieldDefinition[],
        value: string[] 
    }>();

    const field = ref(null as null | SearchFieldDefinition );
    const readOnly = computed(() => field.value && field.value.type >  0);

    </script>

<template>
    Field Expression <br />
    <FieldConstraint :searchFields="props.searchFields" :value="props.value"></FieldConstraint>

</template>