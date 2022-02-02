<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { useStore } from 'vuex';
    import { Guid } from 'guid-typescript'
    import { Option, OptionsField } from '../../models/fieldContainer'
    import { validateOptionsField } from '../../store/form-validators'
    import { FlattenedFormFiledMutations } from '../../store/form-submission-utils'

    export default defineComponent({
		name: "SelectField",
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
            }
        },
        computed: {
            selected: {
                get(): Guid | undefined {
                    return this.model.options.$values.find((opt: Option) => opt.selected)?.id;
                },
                set(value: Guid | null) {
                   
                    this.model.options.$values.forEach((opt: Option) => {
                        this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: opt.id, isSelected: (opt.id === value) });
                    })
                }
            }
        },
        setup(p) {
            const validationStatus = computed(() => validateOptionsField(p.model));
        
            const store = useStore();

            return {
                store,
                validationStatus
              
            }

        }
    });
</script>

<template>
    <div>
        <select v-model="selected" class="form-control col-md-6">
            <option disabled value="">Please select one</option>
            <option v-for="option in model.options.$values" :id="option.id" :value="option.id">{{this.getConcatenatedOptionLabels(option)}}</option>
        </select>
        <!--<span>Selected: {{ selected }}</span>-->
    </div>
</template>
