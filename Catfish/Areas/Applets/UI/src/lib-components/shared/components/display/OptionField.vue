<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { OptionsField, OptionType } from '../../models/fieldContainer'


    export default defineComponent({
        name: "OptionField",
        props: {
            model: {
                type: null as PropType<OptionsField> | null,
                required: true
           },
            type: 
            {
                type: Object as PropType<OptionType>,
                required: true
               
            }

        }
    });
</script>

<template>
    <div v-if="type == OptionType.Checkbox">
        <div v-for="(option, opIndex) in model.options" :key="option">
            <input type="checkbox" id="option" value="option" v-model="model.options[optIndex]" v-selected="option.selected">
            <label for="option">
                <span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>
            </label>
        </div>
    </div>
    <div v-if="type == OptionType.Radio">
        <div v-for="(option, optIndex) in model.options" :key="option.id">
            <input type="radio" id="option.id" value="option.id" v-model="model.options[optIndex]" v-selected="option.selected">
            <label for="option.id">
                <span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>
            </label>
        </div>
    </div>
    <div v-if="type == OptionType.Select">
        <select name="model.id" id="model.id" >
            <option v-for="(option, optIndex) in model.options" :key="option.id" value="option.id"> 
                 <span v-for="text in option.optionText" :key="text.id">{{text.concatenatedContent}}</span>
            </option>
           
        </select>
    </div>
</template>

