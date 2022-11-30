
<script setup lang="ts">
    import { ref } from 'vue'
    import { Guid } from "guid-typescript";
    import { VueDraggableNext as draggable } from 'vue-draggable-next'

    import { Field, TextCollection as TextCollectionModel, FieldType, TextType, MonolingualFieldType, Option } from '../../shared/form-models';
    import { isOptionField, createTextCollection, createOption, cloneTextCollection, isTextInputField, cloneOption } from '../../shared/form-helpers'
    import { default as TextCollection } from './TextCollection.vue'
    import { default as Opt } from './Option.vue'
    import { useFormBuilderStore } from '../store'

/* import the fontawesome core */
    import { library } from '@fortawesome/fontawesome-svg-core'
    
    /* import font awesome icon component */
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

    /* import specific icons */
    import * as faIcons from '@fortawesome/free-solid-svg-icons'
    /* add icons to the library */
    library.add(faIcons.faCircleCheck)
    library.add(faIcons.faCircleXmark)
    library.add(faIcons.faPenToSquare)
    library.add(faIcons.faCirclePlus)
    library.add(faIcons.faQuestionCircle)
    library.add(faIcons.faThList)
    library.add(faIcons.faArrowLeft)

    

   const props = defineProps<{ model: Field }>();
    const isAnOptionField = isOptionField(props.model);

    const store = useFormBuilderStore();
    const newOptionModel = ref(createOption(store.lang, cloneTextCollection(createTextCollection(store.lang) as TextCollectionModel)));

    const addOption = () => {
        props.model.options!.push(cloneOption(newOptionModel.value as Option))

        //Resetting the new option model properties
        newOptionModel.value.isExtendedInput = false;
        newOptionModel.value.isExtendedInputRequired = false;
        newOptionModel.value.optionText.values.forEach(txt => txt.value = "");
    }

    const deleteOption = (optId: Guid) => {
        const idx = props.model.options?.findIndex(opt => opt.id == optId)
        props.model.options?.splice(idx as number, 1)
    }

//AttachmentFild
    const isAttachmentField = props.model.type === FieldType.AttachmentField ? true: false;
  
</script>

<template>
    <h5>{{model.type}}</h5>
    <b-row >
        <b-col class="col-sm-2">
            <h6>Title:</h6>
        </b-col>
        <b-col class="col-sm-10">
            <TextCollection :model="model.title" :text-type="FieldType.ShortAnswer" />
        </b-col>
    </b-row>
    <b-row>
        <b-col class="col-sm-2">
            <h6>Description:</h6>
        </b-col>
        <b-col class="col-sm-10">
            <TextCollection :model="model.description" :text-type="FieldType.Paragraph" />
        </b-col>
    </b-row>
    <br />
    <div v-if="isAnOptionField">
        <h6>Options:</h6>
        <!--Display the current list of options-->
        <div class="display-options">
            <draggable class="dragArea list-group w-full" :list="model.options">
                <div v-for="option in model.options" :key="option.id" class="option-entry row">
                    <div class="col-10">
                        <Opt :model="option" :option-field-type="model.type" />
                    </div>
                    <div class="col-2">
                        <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteOption(option.id)" class="fa-icon delete" />
                    </div>
                </div>
            </draggable>
        </div>

        <!--Allow adding a new option to the list-->
        <div class="alert alert-success">
            <h6>Add Option</h6>
            <Opt :model="newOptionModel" :option-field-type="model.type" :disable-inline-editing="true" />
            <!--<b-row>
                <b-col class="col-sm-3">
                    <h6>Extended Input Field:</h6>
                </b-col>
                <b-col class="col-sm-9">
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
                </b-col>
            </b-row>-->
            <!--<br />
            <b-row v-if="model.options.isExtendedInput">
                <b-col class="col-sm-3">
                    <h6>Extended Input Required Field:</h6>
                </b-col>
                <b-col class="col-sm-9">
                    <br />
                    <div class="toggle-button-cover">
                        <div class="button-cover">
                            <div class="button r" id="button-1">
                                <input v-model="model.options.isExtendedInputRequired" type="checkbox" class="checkbox" />
                                <div class="knobs"></div>
                                <div class="layer"></div>
                            </div>
                        </div>
                    </div>
                </b-col>
            </b-row>-->
            <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addOption()" class="fa-icon plus add-option" />
        </div>

    </div>
    <div class="row">
        <div class="col-sm-2">
            <h6>Required Field:</h6>
        </div>
        <div class="col-sm-10">
            <br />
            <div class="toggle-button-cover">
                <div class="button-cover">
                    <div class="button r" id="button-1">
                        <input v-model="model.isRequired" type="checkbox" class="checkbox" />
                        <div class="knobs"></div>
                        <div class="layer"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br />
    <div class="row" v-if="isTextInputField(model)">
        <div class="col-sm-2">
            <h6>Multiple Value Field:</h6>
        </div>
        <div class="col-sm-10">
            <br />
            <div class="toggle-button-cover">
                <div class="button-cover">
                    <div class="button r" id="button-1">
                        <input v-model="model.isMultiValued" type="checkbox" class="checkbox" />
                        <div class="knobs"></div>
                        <div class="layer"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    
</template>

<style scope>
    .form-field:hover {
        background-color: #F0F0F0 !important;
    }
    .option-entry{
        margin-bottom: 15px;
        padding: 10px;
    }
    .option-entry:hover {
        border: solid 1px #808080;
    }
    
</style>