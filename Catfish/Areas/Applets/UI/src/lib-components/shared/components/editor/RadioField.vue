<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
   
	import { OptionsField, OptionsFieldMethods } from '../../models/fieldContainer'
    import Radio from './option/Radio.vue'
    import { validateOptionsField } from '../../store/form-validators'

    export default defineComponent({
		name: "RadioField",
        components: {
            Radio
        },
        props: {
            model: {
                type: Object as PropType<OptionsField> | null,
                required: true
           }
        },
        methods: {
            getSelectedFieldLabels(field: OptionsField) {
                return OptionsFieldMethods.getSelectedFieldLabels(field.options.$values);
            }
        },
        setup(p) {
            const validationStatus = computed(() => validateOptionsField(p.model));
            //const type = p.model.modelType;
            const name = "radio_" + p.model.id;
            return {

                validationStatus,
               // type
               name

            }

        }
    });
</script>

<template>
    <!--<div>Radio Field</div>
    <div>{{JSON.stringify(model.options.$values)}}</div>-->
    <div v-for="option in model.options.$values">
        <Radio :model="option" :validation-status="validationStatus" :name="name" />
    </div>
    {{JSON.stringify(model.selectedOptionGuids)}}
</template>
