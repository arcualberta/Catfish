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
        setup(p) {
            const formStore = useFormSubmissionStore();

            return {
                setOptionSelection: (id: Guid, selected: boolean) => {
                    //Clearing any previous selections of all other options
                    p.model?.options.$values.filter(opt => opt.id !== id).forEach(opt => formStore.setOptionSelection(opt.id, false));

                    //setting the selected option
                    formStore.setOptionSelection(id, selected)
                }
            }
        }
    });
</script>


<template>
     <div v-for="option in model.options.$values" :key="option.id">
        <input type="radio" :id="option.id" :value="option.id" v-model="selected" @change="setOptionSelection(option.id, $event.target.checked)" /> <label :for="option.id">{{this.getConcatenatedOptionLabels(option)}}</label>
         Selected: {{option.selected}}
    </div>
</template>
