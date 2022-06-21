<script lang="ts">
    import { defineComponent, PropType, computed, ref } from "vue";
    import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'


    export default defineComponent({
        name: "OptionInput",
        props: {
            model: {
                type: null as PropType<models.Option> | null,
                required: false
            },
            fieldType: {
                type: null as PropType<string> | null,
                require: false,
                default: "checkbox"
            },
            name: {
                type: null as PropType<string> | null,
                require: false,
                default: null
            },
       },
        setup(p) {
            const formStore = useFormSubmissionStore();

            const extendedValueInput = ref("");

            return {
                concatenatedOptionLabel: computed(() => p.model?.optionText?.values?.$values.map((txt: { value: any; }) => txt.value).join(" / ")),
                setOptionSelection: (id: Guid, selected: any) => formStore.setOptionSelection(id, selected),
                extendedValues: computed(() => p.model?.extendedValues?.$values),
                extendedValueInput,
                addExtendedValue: () => {
                    if (p.model)
                        formStore.addExtendedOptionValue(p.model.id, extendedValueInput.value);
                    extendedValueInput.value = "";
                },
                removeExtendedValue: (index: number) => {
                    if (p.model)
                        formStore.removeExtendedOptionValue(p.model.id, index);
                }            }
        }
    });
</script>


<template>
    <input :type="fieldType" :name="name" :id="model.id" :value="model.id" @change="setOptionSelection(model.id, $event.target.checked)" />
    <label :for="model.id"> {{concatenatedOptionLabel}}</label>
    <div v-if="model.extendedOption && model.selected">
        <ul>
            <li v-for="(val, index) in extendedValues" :key="index">
                {{val}}
                <span class="fa remove-circle" @click="removeExtendedValue(index)"> x </span>
            </li>
        </ul>
        <br />
        <input type="text" :id="model.id + '_extended'" v-model="extendedValueInput" />
        <button @click="addExtendedValue">Add</button>
    </div>
</template>
