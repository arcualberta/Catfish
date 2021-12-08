<script lang="ts">
    import { defineComponent, PropType } from "vue";
    import { FieldContainer } from "../../shared/models/fieldContainer";

    export default defineComponent({
        name: "NotificationEditor",
        props: {
            fieldContainer: {
                required: false,
                type: null as PropType<FieldContainer> | null
            }
        },
        setup() {

        }

    });

</script>

<template>
    <h4>{{fieldContainer.name.concatenatedContent}}</h4>
    <div>{{fieldContainer}}</div>
</template>