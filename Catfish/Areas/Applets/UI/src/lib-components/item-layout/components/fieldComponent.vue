<script lang="ts">
    import { defineComponent, PropType, computed  /*, ref  */} from 'vue'

    import { ComponentField } from "../models/componentField"
    import {/* Field,*/ MultilingualTextField } from "../../shared/models/fieldContainer"

  
   
   export default defineComponent({
            name: "FieldComponent",
       components: {
         
         
        },
       props: {
           model: {
               type: null as PropType<ComponentField> | null,
               required: true
           },
       },
        setup(p) {
            return {
                htmlWrapperTag: computed(() => p.model.component.type?.length > 0 ? p.model.component.type : "div"),
				componentType: computed(() => p.model.component.$type.split(',')[0]),
                field: computed(() => p.model.field ),
				fieldType: computed(() => p.model.field.$type.split(',')[0]),
                multiTextField: computed(() => (p.model.field as MultilingualTextField))
            }
        }
       
    });
</script>

<template>

   
    <div class="fieldType">
        {{JSON.stringify(model)}} <br />
        {{htmlWrapperTag}}: {{compType}}
        <hr />
        <component :is="htmlWrapperTag" v-if="componentType === 'Catfish.Areas.Applets.Models.Blocks.ItemLayout.StaticText'"> {{model.component.content}} </component>
        <div  v-else v-if="fieldType === 'Catfish.Core.Models.Contents.Fields.TextArea' || fieldType === 'Catfish.Core.Models.Contents.Fields.TextField'">
			<div v-for="multilingualValue in field.values.$values">
                <component :is="htmlWrapperTag" v-for="valueInOneLanguage in multilingualValue.values.$values">
                    {{valueInOneLanguage.value}}
                </component>
			</div>
        </div>
        <br />
        <hr />
        <hr />
        <!--<component :is="type"> {{field[0].values.$values[0].concatenatedContent}} </component>-->
    </div>
</template>

