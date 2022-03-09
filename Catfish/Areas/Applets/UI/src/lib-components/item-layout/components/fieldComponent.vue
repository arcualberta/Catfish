<script lang="ts">
    import { defineComponent, PropType, computed  /*, ref  */} from 'vue'

    import { ComponentField } from "../models/componentField"
  //  import {/* Field,*/ MultilingualTextField } from "../../shared/models/fieldContainer"

  
   
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
            //  const store = useStore();
          //  const field = p.model.field;
          //  const layout = p.model.component;
            
            //  console.log("field in fieldComponent: " + JSON.stringify(field));
            //const type = p.model.component.type;
            return {
               // model: computed(() => p.model)
                 //   store,
            //   field,
              //  layout
                type: computed(() => p.model.component.type),
                field: computed(() => p.model.field )
                //textValues: computed(() => (p.model.field as MultilingualTextField).values?.$values )
            }
        }
       
    });
</script>

<template>
   
    <div class="fieldType">
        <label>{{field[0].name.concatenatedContent}}</label>
        <component :is="type"> {{field[0].values.$values[0].concatenatedContent}} </component>

  </div>
</template>

