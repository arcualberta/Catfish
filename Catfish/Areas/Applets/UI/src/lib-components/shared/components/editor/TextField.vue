<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { MultilingualTextField } from '../../models/fieldContainer'
    import TextCollection from './text/TextCollection.vue'
    import TextArea from './text/textArea.vue'
    import TextInput from './text/text.vue'

    export default defineComponent({
        name: "TextField",
        props: {
            model: {
                type: null as PropType<MultilingualTextField> | null,
                required: true
           },
            isMultivalued: 
            {
                type: Boolean,
                required: false,
                default: true
            },
            isMultiline: {
                type: Boolean,
                required: false,
                default: true
            }

        },
        components: {
            TextCollection,
            TextArea,
            TextInput
        },
        setup(p) {

            const type = p.model.modelType;
            return {
                type
            }
        }
    });
</script>

<template>
    <!--<div>{{type}} : {{JSON.stringify(model)}}</div>-->
    <label>{{model.name.concatenatedContent}} <span v-if="model.required" class="requiredField"></span></label>
    <div v-if="type.includes('Catfish.Core.Models.Contents.Fields.TextArea')">

        <TextArea v-for="val in model.values"  :key="val.id" :isRequired="model.required" />
    </div>
    <div v-else>
      <TextInput  v-for="val in model.values"  :key="val.id" :isRequired="model.required" />
    </div>

    
</template>

