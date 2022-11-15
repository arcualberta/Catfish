<script setup lang="ts">
    import { computed, ref, watch } from 'vue'
    import { Guid } from "guid-typescript";
    import { FieldEntry, FormTemplate } from '../../form-models';
    import { FormEntry } from '@/components/shared';
    import { getFieldTitle } from '@/components/shared/form-helpers'
    import { OptionEntry, SelectableOption, OptionGroup } from './models'

    const props = defineProps<{
        model: FieldEntry,
        optionSource : {
            formGroupName: string,
            formGroup: FormEntry[]
        }[],
        forms: FormTemplate[]
    }>();

    const formSelectionOptions = computed(() => {
        const opts: OptionEntry[] = [{ value: null, text: 'Please select a form' } as unknown as SelectableOption];
        props.optionSource.forEach(group => {
            const groupOpt: OptionGroup = {
                label: group.formGroupName,
                options: [] as OptionEntry[]
            };

            group.formGroup.forEach(form => {
                groupOpt.options.push({
                    value: form.id,
                    text: form.name
                });
            });
            opts.push(groupOpt);
        })
        return opts;
    });

    const fieldSelectionOptions = computed(() => {
        if (props.model?.formId === null || props.model?.formId?.toString() === "")
            return [{ value: null, text: "Please select a form first." }]

        const options = props.forms?.filter(form => form.id === props.model?.formId)[0]
            ?.fields.map(field => {
                return {
                    value: field.id,
                    text: getFieldTitle(field, null)
                } as SelectableOption
            });
        return options;
    });

    watch(() => props.optionSource, newSource => {
        let matchFound = false;
        newSource.forEach(source => {
            if(source.formGroup.filter(formEntry => formEntry.id === props.model.formId).length > 0){
                matchFound = true;
            }
        })

        if(!matchFound){
            console.log("Resetting fields ", newSource)
            props.model.formId = Guid.EMPTY as unknown as Guid;
            props.model.fieldId = Guid.EMPTY as unknown as Guid;

        }
    }, {deep:true})

</script>

<template>
    <div class="row">
        <div class="col-6">
            <b-form-select v-model="model.formId" :options="formSelectionOptions"></b-form-select>
        </div>

        <div class="col-6">
            <b-form-select v-model="model.fieldId" :options="fieldSelectionOptions"></b-form-select>
        </div>
    </div>
</template>