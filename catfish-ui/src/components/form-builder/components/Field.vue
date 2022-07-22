
<script setup lang="ts">
    import { ref } from 'vue'
    import { Guid } from "guid-typescript";
    import { Field, FieldTypes, TextCollection as TextCollectionModel } from '../../shared/form-models';
    import { isOptionField, createTextCollection, createOption, cloneTextCollection } from '../../shared/form-helpers'
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Opt } from './Option.vue'
    import { useFormEditorStore } from '../store'

    const props = defineProps<{ model: Field }>();
    const isAnOptionField = isOptionField(props.model);

    const store = useFormEditorStore();
    const newOptionInput = ref(createTextCollection(store.lang))

    const addOption = () => {
        props.model.options?.push(createOption(store.lang, cloneTextCollection(newOptionInput.value as TextCollectionModel)))
        newOptionInput.value.values.forEach(val => { val.value = "" })
    }

    const deleteOption = (optId: Guid) => {
        const idx = props.model.options?.findIndex(opt => opt.id == optId)
        props.model.options?.splice(idx as number, 1)
    }

</script>

<template>
    <h5>{{model.type}}</h5>
    <div>
        <h6>Title:</h6>
        <TextCollection :model="model.title" :text-type="FieldTypes.SingleLine" />
    </div>
    <div>
        <h6>Description:</h6>
        <TextCollection :model="model.description" :text-type="FieldTypes.Paragraph" />
    </div>
    <div v-if="isAnOptionField">
        <h6>Options:</h6>
        <!--Display the current list of options-->
        <div class="display-options">
            <div v-for="option in model.options" :key="option.id">
                <Opt :model="option" :option-type="model.type" />
                <button class="opt-delete" @click="deleteOption(option.id)">X</button>
                <div style="margin-bottom:15px;" />
            </div>
        </div>
        

        <!--Allow adding a new option to the list-->
        <div>
            <TextCollection :model="newOptionInput" :text-type="FieldTypes.SingleLine" />
            <button class="add-option" @click="addOption()">+</button>
        </div>
        
    </div>
</template>

