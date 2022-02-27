<script lang="ts">
	import { Guid } from 'guid-typescript'
    import { defineComponent, PropType, computed } from 'vue'
	import { useStore } from 'vuex';

	import { Option, OptionsField, OptionsFieldMethods } from '../../models/fieldContainer'
    import { validateOptionsField } from '../../store/form-validators'
	import { FlattenedFormFiledMutations } from '../../store/form-submission-utils'

    export default defineComponent({
		name: "RadioField",
        props: {
            model: {
                type: Object as PropType<OptionsField> | null,
                required: true
           }
        },
        methods: {
            getSelectedFieldLabels(field: OptionsField): string {
                return OptionsFieldMethods.getSelectedFieldLabels(field.options.$values);
            },
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
                    //console.log("selected value: ", value);
                    this.model.options.$values.forEach((opt: Option) => {
						this.store.commit(FlattenedFormFiledMutations.SET_OPTION_VALUE, { id: opt.id, isSelected: (opt.id === value) });
					})
				}
			}
		},
        setup(p) {
            const validationStatus = computed(() => validateOptionsField(p.model));
            const name = "radio_" + p.model.id;
			const store = useStore();

            return {
                store,
                validationStatus,
                name
            }

        }
    });
</script>

<template>
    <div v-for="option in model.options.$values">
        <input type="radio" :name="name" :id="option.id" :value="option.id" v-model="selected" /> <label :for="option.id">{{this.getConcatenatedOptionLabels(option)}}</label>
    </div>
    <!--{{JSON.stringify(selected)}}-->
</template>
