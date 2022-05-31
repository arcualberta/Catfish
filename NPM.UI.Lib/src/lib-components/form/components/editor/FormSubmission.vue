<script lang="ts">
    import { defineComponent, PropType, computed, watch } from "vue"
    import { Pinia } from 'pinia'

    import * as models from '../../models'
    import * as helpers from '../../helpers'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'

    import { default as FormField } from './FormField.vue'

    export default defineComponent({
        name: "SubmissionForm",
        props: {
            model: {
                type: null as PropType<models.FieldContainer> | null,
                required: false
            },
            piniaInstance: {
                type: null as PropType<Pinia> | null,
                required: false
            }
        },
        components: {
            FormField,
        },
        setup(p) {

            const formSubmissionStore = useFormSubmissionStore(p.piniaInstance);

            formSubmissionStore.form = p.model as models.FieldContainer;

            watch(() => p.model, async newModel => {
                formSubmissionStore.form = newModel as models.FieldContainer;
            })

            return {
                name: computed(() => helpers.getFieldName(p.model as models.FieldContainer)),
            }
        },
    });
</script>

<template>
    <h2>{{name}}</h2>
    <FormField v-for="field in model?.fields.$values" :key="field.id" :model="field" />
    <!--{{JSON.stringify(model)}}-->
</template>
