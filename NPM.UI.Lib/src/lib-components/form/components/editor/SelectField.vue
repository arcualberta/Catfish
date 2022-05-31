<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { Guid } from 'guid-typescript';

    import * as models from '../../models'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'

    export default defineComponent({
		name: "SelectField",
        components: {
            
        },
        props: {
            model: {
                type: Object as PropType<models.OptionsField> | null,
                required: true
           }
        },
        methods: {
           
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
    <div>
        <select v-model="selected" class="form-control col-md-6">
            <option disabled value="">Please select one</option>
            <option v-for="option in model.options.$values" :id="option.id" :value="option.id" :key="option.id"  @change="setOptionSelection(option.id, option.seledcted)" >{{this.getConcatenatedOptionLabels(option)}}</option>
            Selected: {{option.selected}}
        </select>
       
    </div>
</template>
