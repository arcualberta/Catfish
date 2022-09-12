
<script setup lang="ts">
    import { ref } from 'vue'
    import { Guid } from "guid-typescript";
    import { VueDraggableNext as draggable } from 'vue-draggable-next'

    import { Field, TextCollection as TextCollectionModel, FieldType } from '../../shared/form-models';
    import { isOptionField, createTextCollection, createOption, cloneTextCollection } from '../../shared/form-helpers'
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Opt } from './Option.vue'
    import { useFormBuilderStore } from '../store'

    const props = defineProps<{ model: Field }>();
    const isAnOptionField = isOptionField(props.model);

    const store = useFormBuilderStore();
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
        <TextCollection :model="model.title" :text-type="FieldType.ShortAnswer" />
    </div>
    <div>
        <h6>Description:</h6>
        <TextCollection :model="model.description" :text-type="FieldType.Paragraph" />
    </div>
    <div v-if="isAnOptionField">
        <h6>Options:</h6>
        <!--Display the current list of options-->
        <div class="display-options">
            <draggable class="dragArea list-group w-full" :list="model.options">
                <div v-for="option in model.options" :key="option.id" class="option-entry">
                    <Opt :model="option" :option-type="model.type" />
                    <span><font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteOption(option.id)" class="fa-icon delete" /></span>
                </div>
            </draggable>
        </div>

        <!--Allow adding a new option to the list-->
        <div>
            <TextCollection :model="newOptionInput" :text-type="FieldType.ShortAnswer" />
            <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addOption()" class="fa-icon plus add-option"/>
        </div>
        
    </div>
</template>

<style scope>
    .form-field:hover {
        background-color: #F0F0F0;
    }
    .option-entry{
        margin-bottom: 15px;
        padding: 10px;
    }
    .option-entry:hover {
        border: solid 1px #808080;
    }
</style>