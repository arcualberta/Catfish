<script setup lang="ts">
    import { ref, computed } from 'vue'
    import { Guid } from "guid-typescript";
    import { Field, TextCollection as TextCollectionModel, FieldType, TextType, MonolingualFieldType, Option } from '../../shared/form-models';
    import { FormEntry, FieldEntry } from '../../form-models';
    import { isString } from '@vue/shared';
    import { OptionEntry, SelectableOption, OptionGroup } from './models'

    const props = defineProps<{
        model: FieldEntry,
        optionSource : {
            formGroupName: string,
            formGroup: FormEntry[]
        }[]
    }>();
    
    const id = ref(Guid.create().toString() as unknown as Guid);
    const selected = ref("");

    const options = computed(() => {
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
</script>

<template>
    <b-form-select v-model="selected" :options="options" class="col-6"></b-form-select>
    <div class="alert alert-info" style="margin-top:2em;">{{selected}}</div>
</template>