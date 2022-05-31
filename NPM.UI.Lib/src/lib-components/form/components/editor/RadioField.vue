<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'

    export default defineComponent({
        name: "RadioField",
        props: {
            model: {
                type: null as PropType<models.OptionsField> | null,
                required: false
            },
        },

        methods:{
             getConcatenatedOptionLabels(option: any): string {
                const concatenatedLabels = option.optionText?.values?.$values.map((txt: { value: any; }) => txt.value).join(" / ")
                return concatenatedLabels ? concatenatedLabels : "";
			}
        },
        setup() {
            const formStore = useFormSubmissionStore();

            return {
                setOptionSelection: (id: Guid, selected: boolean) => formStore.setOptionSelection(id, selected)
            }
        }
    });
</script>


<template>
     <div v-for="option in model.options.$values" :key="option.id">
        <input type="radio" :name="name" :id="option.id" :value="option.id" v-model="selected" /> <label :for="option.id">{{this.getConcatenatedOptionLabels(option)}}</label>
         Selected: {{option.selected}}
    </div>
</template>
