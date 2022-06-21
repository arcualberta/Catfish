<script lang="ts">
    import { defineComponent, PropType } from "vue"
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import * as models from '../../models'
    import TextCollectionInput from "./TextCollectionInput.vue"

    export default defineComponent({
        name: "TextField",
        components:{
            TextCollectionInput
        },
        props: {
            model: {
                type: null as PropType<models.MultilingualTextField> | null, //PropType<models.TextField>
                required: false
            },
        },
        setup() {
            const formStore = useFormSubmissionStore();

            return {
                formStore
            }
        }
        
    });
</script>


<template>
    <div v-for="(val in model.values.$values" :key="val.id" class="multilingualField">
        <TextCollectionInput :model="val" :is-multiline="false" />
        <span v-if="model?.values?.$values?.length > 1" class="fa remove-circle" style="margin-left: 30%; padding-right: 3px; padding-left: 2px; padding-bottom: 6px;" @click="formStore.removeMutilingualValue(model, val.id)"> x </span>
    </div>
</template>
