<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";
    import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'


    export default defineComponent({
        name: "Checkbox",
        props: {
            model: {
                type: null as PropType<models.Option> | null,
                required: false
            },
        },
        setup(p) {
            const formStore = useFormSubmissionStore();


            const extendedValue = computed({
                get: () => p.model?.extendedValue,
                set: (value) => formStore.setExtendedOptionValue(p.model?.id as Guid, value as string),
            })

            return {
                concatenatedOptionLabel: computed(() => p.model?.optionText?.values?.$values.map((txt: { value: any; }) => txt.value).join(" / ")),
                setOptionSelection: (id: Guid, selected: any) => formStore.setOptionSelection(id, selected),
                extendedValue
            }
        }
    });
</script>


<template>
    <input type="checkbox" :id="model.id" :value="model.id" @change="setOptionSelection(model.id, $event.target.checked)" />
    <label :for="model.id"> {{concatenatedOptionLabel}}</label>
    <input v-if="model.extendedOption && model.selected" type="text" :id="model.id + '_extended'" :value="extendedValue" />
</template>
