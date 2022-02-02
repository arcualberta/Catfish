<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
  // import { Guid } from 'guid-typescript'
    import { useStore } from 'vuex';
    import { Option, OptionsField/*, OptionsFieldMethods*/ } from '../../models/fieldContainer'
   // import { validateOptionsField } from '../../store/form-validators'
   import { FlattenedFormFiledMutations } from '../../store/form-submission-utils'

    export default defineComponent({
        name: "CheckboxField",
        components: {

        },
        props: {
            model: {
                type: Object as PropType<OptionsField> | null,
                required: true
            }
        },
        methods: {
         
            getConcatenatedOptionLabels(option: Option): string {
                const concatenatedLabels = option.optionText?.values?.$values.map(txt => txt.value).join(" / ")
                return concatenatedLabels ? concatenatedLabels : "";
            },

            selectoption(event: any) {
               // console.log("selected value " + event.target.value + "selected: " + JSON.stringify(event.target.checked));        
                this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: event.target.value, isSelected: event.target.checked });
            }
        },
        setup(p) {
           // const validationStatus = computed(() => validateOptionsField(p.model));
            
            const store = useStore();

            return {
                store,
               // validationStatus,
				selectedoptions: computed(() => p.model.options.$values.filter((opt) => opt.selected === true).map(opt => opt.id))
              
            }

        }
    });
</script>

<template>
    <div v-for="option in model.options.$values">
        <input type="checkbox" :id="option.id" :value="option.id" v-model="selectedoptions" @change="selectoption($event)" />
        <label :for="option.id"> {{this.getConcatenatedOptionLabels(option)}}</label>
    </div>
    <!--Selected chekboxes: {{JSON.stringify(selectedoptions)}}-->
</template>
