<script lang="ts">
    import { defineComponent, PropType, computed  /*, ref , computed */} from 'vue'
//import { MultilingualTextField } from '../../shared/models/fieldContainer';
    import { ComponentField } from "../models/componentField"
 // import { Field, MultilingualTextField } from "../../shared/models/fieldContainer"


   export default defineComponent({
            name: "FieldComponent",
        components: {

        },
        props: {
            model: {
            type: null as PropType<ComponentField> | null,
            required: true
            },
            //fieldlayout:{
            //    type: null as PropType<ComponentLayout> | null,
            //    required: false
            //}

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
                type: computed(() => p.model.component.type)
            }
        }
       
    });
</script>

<template>
   
    <div class="fieldType">
        <h3>Field Component</h3>
        {{JSON.stringify(model.field)}}
        <h3>HTML field type</h3>
        {{type}}
      
   
    </div>
</template>

