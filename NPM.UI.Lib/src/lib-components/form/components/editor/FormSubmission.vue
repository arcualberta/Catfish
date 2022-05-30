<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";

    import * as models from '../../models'
    import * as helpers from '../../helpers'

    import { default as Field } from './Field.vue'

    export default defineComponent({
        name: "SubmissionForm",
        props: {
            model: {
                type: null as PropType<models.FieldContainer> | null,
                required: false
            },
        },
        components: {
            Field,
        },
        setup(p) {

           return{
               name: computed(() => helpers.getFieldName(p.model as models.FieldContainer)),
           }
        },
    });
</script>

<template>
    <h2>{{name}}</h2>
    <Field v-for="field in model.fields.$values" :key="field.id" :model="field" />
    <!--{{JSON.stringify(model)}}-->
</template>
