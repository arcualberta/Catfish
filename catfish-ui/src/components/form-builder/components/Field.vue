<script lang="ts">
    import { defineComponent } from "vue";
    import { default as TextCollection } from './TextCollection.vue'

    export default defineComponent({
        name: "Field",
        components: {
            TextCollection
        }
    });
</script>

<script setup lang="ts">
    import { Field } from '../../shared/form-models';

    const props = defineProps<{ model: Field }>();

</script>

<template>
    <h4>{{model.type}}</h4>
    <div>
        Name:
        <TextCollection v-if="model.name" :model="model.name" />
        <button v-else>Set name</button>
    </div>
    <div>
        Description:
        <TextCollection v-if="model.descrption" :model="model.descrption" />
        <button v-else>Set description</button>
    </div>
</template>

