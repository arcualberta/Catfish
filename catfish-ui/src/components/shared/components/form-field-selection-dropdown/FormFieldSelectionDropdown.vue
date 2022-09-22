<script setup lang="ts">
    import { ref, computed } from 'vue'
    import { Guid } from "guid-typescript";
    import { Field, TextCollection as TextCollectionModel, FieldType, TextType, MonolingualFieldType, Option } from '../../shared/form-models';
    import { FormEntry } from '../../form-models';

    const props = defineProps<{
        model: {
            formGroupName: string,
            formGroup: FormEntry[]
        }[]
    }>();

    const id = ref(Guid.create().toString() as unknown as Guid);
    const selected = ref("");

    const options = computed(() => {
        const opts = [{ value: null, text: 'Please select a form' }];
        props.model.forEach(group => {
            const groupOpt = {
                label: group.formGroupName,
                options: []
            }
            console.log("Form Name: ", group.formGroupName)
            console.log("Form Group: ", JSON.stringify(group.formGroup))

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
    {{selected}}
</template>