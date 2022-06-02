<script lang="ts">
    import { defineComponent, PropType, computed } from "vue";

    import * as models from '../../models'
    import * as helpers from '../../helpers'

    import { default as InfoSection } from './InfoSection.vue'

    export default defineComponent({
        name: "Field",
        props: {
            model: {
                type: null as PropType<models.Field> | null,
                required: true
            },
        },
        components: {
            InfoSection
        },
        setup(p) {

            return {
                isInfoSection: () => p.model.$type.startsWith('Catfish.Core.Models.Contents.Fields.InfoSection'),
                name: computed(() => helpers.getFieldName(p.model as models.Field)),
            }
        },
    });
</script>

<template>
    <div v-if="isInfoSection">
        <InfoSection :model="model" />
    </div>
    <div v-if="!isInfoSection">
        <h3>{{name}}</h3>
        {{JSON.stringify(model)}}
    </div>

</template>
