<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";

    import { useFormEditorStore } from './store';

    //import { Form } from '../shared/form-models';

    export default defineComponent({
        name: "FormEditor",
        components: {
        },
        props: {
            piniaInstance: {
                type: null as PropType<Pinia> | null,
                required: true
            },
            repositoryRoot: {
                type: null as PropType<string> | null,
                required: true
            },
            formId: {
                type: null as PropType<Guid> | null,
                required: false
            },
        },
        setup(p) {
            const store = useFormEditorStore(p.piniaInstance);

            if (p.formId) { }
            console.log(JSON.stringify(store))

            return {
                store,
                newForm: () => store.newForm()
            }
        }

    });
</script>


<template>
    <h2>Form Editor Component</h2>
    <button @click="newForm">New Form</button>

    <hr />
    {{store.form}}
</template>
