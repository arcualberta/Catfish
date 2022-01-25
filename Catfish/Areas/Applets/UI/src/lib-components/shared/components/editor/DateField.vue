<script lang="ts">
    import { defineComponent, PropType, computed} from 'vue'
    //import dayjs from "dayjs";
    import Text from './text/Text.vue'
   import { validateMonolingualTextField /*, RegExpressions*/ } from '../../store/form-validators'
    import { MonolingualTextField } from '../../models/fieldContainer'


    export default defineComponent({
        name: "DateField",
        props: {
            model: {
                type: null as PropType<MonolingualTextField> | null,
                required: true
           },
            isMultivalued: 
            {
                type: Boolean,
                required: false,
                default: false
            }

        },
        components: {
            Text
        },
        setup(p) {

            const validationStatus = computed(() => validateMonolingualTextField(p.model, null));
            const type = p.model.modelType;
            return {

                validationStatus,
                type

            }

        }
        //methods:{
        //    formatDate(dateString: string) {
        //        const date = dayjs(dateString);
        //        return date.format('MMM DD, YYYY');
        //    }
        //}
       
    });
</script>

<template>
   
    <div v-for="val in model?.values?.$values">
        <Text :model="val" :is-multiline="false" :is-rich-text="false" :validation-status="validationStatus" field="date" />
    </div>

</template>

