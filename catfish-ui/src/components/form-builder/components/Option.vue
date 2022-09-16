
<script setup lang="ts">
    import { ref } from 'vue'

    import { Option, OptionFieldType, FieldType } from '../../shared/form-models';
    import { useFormBuilderStore } from '../store';
    import { default as TextCollection } from './TextCollection.vue'

    const props = defineProps<{ model: Option, optionType: OptionFieldType }>();
    const store = useFormBuilderStore();

    const editMode = ref(false)

</script>

<template>
    <div v-if="!editMode">
        <span v-for="value in model.optionText.values">
            <span v-if="value.value?.length! > 0" class="option-values"> {{value.value}} </span>
        </span>
        <font-awesome-icon icon="fa-solid fa-pen-to-square" @click="editMode = true" class="fa-icon" />
    </div>
    <div v-else>
        <TextCollection :model="model.optionText" :text-type="FieldType.ShortAnswer" />
        <h6>Custom Option Field:</h6>
        <div class="col-sm-10">
            <br />
            <div class="toggle-button-cover">
                <div class="button-cover">
                    <div class="button r" id="button-1">
                        <input v-model="model.allowCustomOptionValues" type="checkbox" class="checkbox" />
                        <div class="knobs"></div>
                        <div class="layer"></div>
                    </div>
                </div>
            </div>
        </div>
        <font-awesome-icon icon="fa-solid fa-circle-check" @click="editMode = false" class="fa-icon delete-button" />
    </div>
</template>

