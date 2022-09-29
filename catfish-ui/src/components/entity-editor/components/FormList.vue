<script setup lang="ts">
    import { Pinia } from 'pinia'
    import {ref} from "vue"
    import { useEntityEditorStore } from "../store"
    import { Form } from '../../shared/form-models'
    import {EntityTemplate} from '../../entity-template-builder/models'

    const store = useEntityEditorStore();
    const props = defineProps<{
        forms: Form[],
    }>();
    let selectedform = ref(props.forms[0].id);
</script>

<template>
    <div v-if="forms">
        <span v-for="form in forms" :key="form.id" :model="form"><a @click="selectedform=form.id">{{form.name}}</a></span>
    </div>
</template>
