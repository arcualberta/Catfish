<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";

    import * as models from '../../models'
    import * as helpers from '../../helpers'
    import { eFieldType } from '../../enumerations'

    import { default as InfoSection } from './InfoSection.vue'
    import { default as TextField } from './TextField.vue'
    import { default as AttachmentField } from './AttachmentField.vue'
    import { default as RadioField } from './RadioField.vue'
    import { default as CheckboxField } from './CheckboxField.vue'
    import { default as TextArea } from './TextArea.vue'
    import { default as DateField } from './DateField.vue'
    import { default as DecimalField } from './DecimalField.vue'
    import { default as EmailField } from './EmailField.vue'
    import { default as IntegerField } from './IntegerField.vue'

    export default defineComponent({
        name: "FormField",
        props: {
            model: {
                type: null as PropType<models.Field> | null,
                required: true
            },
        },
        components: {
            InfoSection,
            TextField,
            AttachmentField,
            RadioField,
            CheckboxField,
            TextArea,
            DateField,
            DecimalField,
            EmailField,
            IntegerField
        },
        setup(p) {

            return {
                eFieldType,
                helpers,
                name: computed(() => helpers.getFieldName(p.model as models.Field)),
            }
        },
    });
</script>

<template>
    <div v-if="helpers.testFieldType(model, eFieldType.InfoSection)">
        <InfoSection :model="model" />
    </div>
    <div v-else class="container">
        
        <span v-if="model.required" class="fieldName required">{{name}}</span>
        <span v-else class="fieldName">{{name}}</span>
        <AttachmentField v-if="helpers.testFieldType(model, eFieldType.AttachmentField)" :model="model" />
        <RadioField v-if="helpers.testFieldType(model, eFieldType.RadioField)" :model="model" />
        <CheckboxField v-if="helpers.testFieldType(model, eFieldType.CheckboxField)" :model="model" />
        <TextArea v-if="helpers.testFieldType(model, eFieldType.TextArea)" :model="model" />
        <TextField v-if="helpers.testFieldType(model, eFieldType.TextField)" :model="model" />
        <DateField v-if="helpers.testFieldType(model, eFieldType.DateField)" :model="model" />
        <DecimalField v-if="helpers.testFieldType(model, eFieldType.DecimalField)" :model="model" />
        <EmailField v-if="helpers.testFieldType(model, eFieldType.EmailField)" :model="model" />
        <IntegerField v-if="helpers.testFieldType(model, eFieldType.IntegerField)" :model="model" />
        
        <span v-if="model?.validationStatus === false" class="validation-error">{{model.validationError}}</span>
    </div>

</template>


<style scoped>
    .validation-error{
        margin: 5px;
        color: red;
    }
</style>
