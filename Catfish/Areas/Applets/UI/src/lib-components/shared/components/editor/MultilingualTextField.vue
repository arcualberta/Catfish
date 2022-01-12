<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { MultilingualTextField } from '../../models/fieldContainer'
    import TextCollection from './text/TextCollection.vue'

    export default defineComponent({
		name: "MultilingualTextField",
        props: {
            model: {
                type: null as PropType<MultilingualTextField> | null,
                required: true
            },
			isMultiline: {
				type: Boolean,
				required: true
			}
        },
        components: {
            TextCollection
        },
        setup(p) {

            const type = p.model.modelType;
			//console.log("p.model: ", JSON.stringify(p.model))

            const isRequired = computed(() => {
                //console.log("p.model.required: ", p.model.required)

                //If the field itself is not a required field, none of its inner fields are
                //required to have any value
                if (!p.model.required)
                    return false;

                //We are here means, this is a required field. This means, we need to make sure
                //the field (which can potentially have multiple values) has at least one value
                //in at least one language.
                let valueFound: boolean = false;
                for (let i = 0; !valueFound && p.model?.values && (i < p.model.values?.$values?.length); ++i) {
                    let txtCollection = p.model?.values?.$values[i];
                    for (let k = 0; !valueFound && txtCollection.values && (k < txtCollection.values?.$values.length); ++k) {
                        valueFound = txtCollection.values?.$values[k]?.value?.trim().length > 0;
					}
                }

                //If no value is found (i.e. valueFound is false), we need to continue to force all child fileds to be
                //"required". In contrast, if at lest one value is found, then the other child fields no longer need
                //to require to hold a value.
                return !valueFound; 
            });

            return {
                type,
				isRichText: computed(() => p.model? p.model.richText : false),
                isRequired
            }
        }
    });
</script>

<template>
    <!--<div>MultilingualValue.isRichText: {{isRichText}}</div>
    <div>{{JSON.stringify(model)}}</div>-->
    <TextCollection v-for="val in model.values?.$values" :model="val" :is-multiline="isMultiline" :is-rich-text="isRichText" :is-required="isRequired" />
    <!--<div>{{type
        }} : {{JSON.stringify(model)}}
    </div>-->
    <!--<label>{{model.name.concatenatedContent}} <span v-if="model.required" class="requiredField"></span></label>
    <div v-if="type.includes('Catfish.Core.Models.Contents.Fields.TextArea')">

        <RichText v-if="model.richText" v-for="val in model.values" :model="val" :key="val.id" :isRequired="model.required" />
        <TextArea v-else v-for="val in model.values" :model="val" :key="val.id" :isRequired="model.required" />
    </div>
    <div v-else>
      <TextInput  v-for="val in model.values" :model="val"  :key="val.id" :isRequired="model.required" />
    </div>-->


</template>

