
<script setup lang="ts">
    import { ref, computed } from 'vue'

    import { Option, OptionFieldType, FieldType } from '../../shared/form-models';
    import { useFormBuilderStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{
        model: Option,
        optionFieldType: OptionFieldType,
        disableInlineEditing?: boolean
    }>();
    const store = useFormBuilderStore();

    const editMode = ref(false)
    const allowExtendedInputs = computed(() => props.optionFieldType == OptionFieldType.Checkboxes || props.optionFieldType == OptionFieldType.RadioButtons)

</script>

<template>
    <div class="alert alert-info">
        <div v-if="editMode || disableInlineEditing">
            <TextCollection :model="model.optionText" :text-type="FieldType.ShortAnswer" />
            <span v-if="allowExtendedInputs">
                <input type="checkbox" v-model="model.isExtendedInput" /> Is Extended
                <input type="checkbox" v-model="model.isExtendedInputRequired" /> Is Required
            </span>
            <!--<h6>Extended Input Field:</h6>
        <div class="col-sm-10">
            <br />
            <div class="toggle-button-cover">
                <div class="button-cover">
                    <div class="button r" id="button-1">
                        <input v-model="model.options.isExtendedInput" type="checkbox" class="checkbox" />
                        <div class="knobs"></div>
                        <div class="layer"></div>
                    </div>
                </div>
            </div>
        </div>-->
            <font-awesome-icon v-if="!disableInlineEditing" icon="fa-solid fa-circle-check" @click="editMode = false" class="fa-icon delete-button" />
        </div>
        <div v-else>
            <span v-for="value in model.optionText.values">
                <span v-if="value.value?.length! > 0" class="option-values"> {{value.value}} </span>
            </span>
            <span v-if="model.isExtendedInput"><font-awesome-icon icon="fa-light fa-input-text" /> [[extended]]<span style="color:red;font-weight:bold;" v-if="model.isExtendedInputRequired">*</span></span>
            <font-awesome-icon icon="fa-solid fa-pen-to-square" @click="editMode = true" class="fa-icon" />
        </div>
    </div>
</template>

