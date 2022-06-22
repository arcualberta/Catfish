<script lang="ts">
    import { defineComponent, PropType } from "vue";

    import * as models from '../../models'
    import { default as OptionInput } from './OptionInput.vue'

    export default defineComponent({
        name: "CheckboxField",
        components: {
            OptionInput
        },
        props: {
            model: {
                type: null as PropType<models.OptionsField> | null,
                required: false
            },
        },
    });
</script>


<template>
    <div v-for="option in model.options.$values" :key="option.id" class="checkbox-container">
        <OptionInput field-type="checkbox" :model="option" />
    </div>
</template>
