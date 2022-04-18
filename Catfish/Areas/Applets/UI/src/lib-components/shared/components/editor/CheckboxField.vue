<script lang="ts">
    import { defineComponent, PropType, computed, WritableComputedRef} from 'vue'
   import { Guid } from 'guid-typescript'
    import { useStore } from 'vuex';
    import { Option, OptionsField/*, OptionsFieldMethods*/ } from '../../models/fieldContainer'
    // import { validateOptionsField } from '../../store/form-validators'
    import { FlattenedFormFiledMutations } from '../../store/flattened-form-field-mutations'

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
        setup(p) {
           // const validationStatus = computed(() => validateOptionsField(p.model));
            
            const store = useStore();


            const selectedoptions: WritableComputedRef<Guid[]> = computed({
                get(): Guid[] { return p.model.options.$values.filter((opt) => opt.selected === true).map(opt => opt.id) },
                set: () => { }
            })

            const selectoption = (event: any) => {
                // console.log("selected value " + event.target.value + "selected: " + JSON.stringify(event.target.checked));        
                store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: event.target.value, isSelected: event.target.checked });
            }

            const getConcatenatedOptionLabels = (option: Option): string => {
                const concatenatedLabels = option.optionText?.values?.$values.map(txt => txt.value).join(" / ")
                return concatenatedLabels ? concatenatedLabels : "";
            }

            return {
                store,
               // validationStatus,
                selectoption,
                selectedoptions,
                getConcatenatedOptionLabels,
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
