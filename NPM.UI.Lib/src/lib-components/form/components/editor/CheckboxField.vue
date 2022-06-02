<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'


    export default defineComponent({
        name: "CheckboxField",
        props: {
            model: {
                type: null as PropType<models.OptionsField> | null,
                required: false
            },
        },
   //      methods:{
   //          getConcatenatedOptionLabels(option: any): string {
   //             const concatenatedLabels = option.optionText?.values?.$values.map((txt: { value: any; }) => txt.value).join(" / ")
   //             return concatenatedLabels ? concatenatedLabels : "";
			//}
   //     },
        setup() {
            const formStore = useFormSubmissionStore();

            const getConcatenatedOptionLabels = (option: any): string => {
                const concatenatedLabels = option.optionText?.values?.$values.map((txt: { value: any; }) => txt.value).join(" / ")
                return concatenatedLabels ? concatenatedLabels : "";
            }

            return {
                setOptionSelection: (id: Guid, selected: any) => formStore.setOptionSelection(id, selected),
                getConcatenatedOptionLabels
            }
        }
    });
</script>


<template>
    <div v-for="option in model.options.$values" :key="option.id" class="checkbox col-md-8">
        <input type="checkbox" :id="option.id" :value="option.id" @change="setOptionSelection(option.id, $event.target.checked)" />
        <label :for="option.id"> {{this.getConcatenatedOptionLabels(option)}}</label>
        Selected: {{option.selected}}
    </div>
</template>
