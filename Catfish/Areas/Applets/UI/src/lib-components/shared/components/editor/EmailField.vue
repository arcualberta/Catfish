<script lang="ts">
    import { defineComponent, PropType, computed } from 'vue'
    import { useStore } from 'vuex';
    import { MonolingualTextField } from '../../models/fieldContainer'
    import Text from './text/Text.vue'
    import { validateMonolingualTextField, RegExpressions } from '../../store/form-validators'
    import { FlattenedFormFiledMutations } from '../../store/flattened-form-field-mutations'

    export default defineComponent({
        name: "EmailField",
        props: {
            model: {
                type: null as PropType<MonolingualTextField> | null,
                required: true
           },
            isMultivalue: 
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
            const store = useStore();
            const validationStatus = computed(() => validateMonolingualTextField(p.model, RegExpressions.Email));
            const type = p.model.modelType;
            return {
                store,
                validationStatus,
                type
                
            }
            
        },

        methods: {
            addEmail(store: any, field: MonolingualTextField) {
                store.commit(FlattenedFormFiledMutations.APPEND_MONOLINGUAL_VALUE, field);
            },
        }
    });
</script>

<template>
 
    <div v-for="val in model?.values?.$values">
        <Text :model="val" :is-multiline="false" :is-rich-text="false" :validation-status="validationStatus" field="email" />
        <span class="fa fa-plus-circle" @click="addEmail(store, model)"></span>
     </div>
   
</template>

