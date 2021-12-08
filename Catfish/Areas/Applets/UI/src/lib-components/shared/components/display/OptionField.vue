<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { eFieldType, OptionsField } from '../../models/fieldContainer'

    import CheckboxField from './CheckboxField.vue'
    




    export default defineComponent({
        name: "OptionField",
        components: {
            CheckboxField
        },
        props: {
            model: {
                type: null as PropType<OptionsField> | null,
                required: true
           },
            type: 
            {
                type: null as PropType<eFieldType> | null,
                required: false
            }

        }
    });
</script>

<template>
    
    <div v-for="option in model.options" :key="option.id">
       
            <CheckboxField :model="option.optionText" />
  
    </div>
    <!-- <input v-for="option in model.options" :key="option" type="checkbox" id="option" value="option" v-model="model.id" v-selected="option.selected"> {{model.id}}-->
    <!--<div v-if="type === eFieldType.CheckboxField">
        <div v-for="(option, opIndex) in model.options" :key="option">
            <input type="checkbox" id="option" value="option" v-model="model.options[optIndex]" v-selected="option.selected">-->
    <!--<label for="option">
        <span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>
    </label>-->
    <!--<TextCollection :model="option" />
        </div>
    </div>
    <div v-if="type === eFieldType.RadioField">
        <div v-for="(option, optIndex) in model.options" :key="option.id">
            <input type="radio" id="option.id" value="option.id" v-model="model.options[optIndex]" v-selected="option.selected">
            <TextCollection :model="option" />-->
    <!--<label for="option.id">
        <span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>
    </label>-->
    <!--</div>
    </div>
    <div v-if="type == eFieldType.SelectField">
        <select name="model.id" id="model.id" >
            <option v-for="(option, optIndex) in model.options" :key="option.id" value="option.id">-->
    <!--<span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>-->
    <!--<TextCollection :model="option" />
            </option>

        </select>
    </div>-->
</template>

