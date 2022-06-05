<script lang="ts">
    import { defineComponent, PropType } from "vue";
    //import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    //import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import { default as Checkbox } from './Checkbox.vue'

    export default defineComponent({
        name: "CheckboxField",
        props: {
            model: {
                type: null as PropType<models.OptionsField> | null,
                required: false
            },
        },
        components: {
            Checkbox
        },
    //    setup() {
    //        const formStore = useFormSubmissionStore();

    //        return {
    //        }
    //    }
    });
</script>


<template>
    <div v-for="option in model.options.$values" :key="option.id" class="checkbox-container">
        <checkbox :model="option" />
    </div>
</template>
