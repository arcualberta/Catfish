<script setup lang="ts">
    import { ref, computed, watch } from 'vue'
    import { Guid } from "guid-typescript";
    import { Form, FormEntry, FieldEntry } from '../../form-models';
    import { isString } from '@vue/shared';
    import { getFieldTitle } from '@/components/shared/form-helpers'
    import { OptionEntry, SelectableOption, OptionGroup } from './models'

    const props = defineProps<{
        model?: FieldEntry,
        optionSource : {
            formGroupName: string,
            formGroup: FormEntry[]
        }[],
        forms: Form[]
    }>();

    const emit = defineEmits<{
        (e: 'update', value: FieldEntry): void
    }>()

    const selectedFormId = ref(null as Guid | null);
    const selectedFieldId = ref(null as Guid | null);

    const formSelectionOptions = computed(() => {
        const opts: OptionEntry[] = [{ value: null, text: 'Please select a form' } as unknown as SelectableOption];
        props.optionSource.forEach(group => {
            const groupOpt: OptionGroup = {
                label: group.formGroupName,
                options: [] as OptionEntry[]
            };

            group.formGroup.forEach(form => {
                groupOpt.options.push({
                    value: form.formId,
                    text: form.name
                });
            });
            opts.push(groupOpt);
        })
        return opts;
    });

    const fieldSelectionOptions = computed(() => {
        if (selectedFormId.value === null || selectedFormId.value?.toString() === "")
            return [{ value: null, text: "Please select a form first." }]

        const options = props.forms?.filter(form => form.id === selectedFormId.value)[0]
            ?.fields.map(field => {
                return {
                    value: field.id,
                    text: getFieldTitle(field, null)
                } as SelectableOption
            });
        return options;
    });

    watch(() => selectedFormId.value, _ => {
        selectedFieldId.value = null;
    })

    watch(() => selectedFieldId.value, _ => emit('update', { formId: selectedFormId.value, fieldId: selectedFieldId.value } as FieldEntry))
</script>

<template>
    <div class="row">
        <div class="col-6">
            <b-form-select v-model="selectedFormId" :options="formSelectionOptions"></b-form-select>
        </div>

        <div class="col-6">
            <b-form-select v-model="selectedFieldId" :options="fieldSelectionOptions"></b-form-select>
        </div>
    </div>
</template>