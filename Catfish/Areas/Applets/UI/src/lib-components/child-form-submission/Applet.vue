<script lang="ts">
    import { defineComponent } from 'vue';
    import props from '../shared/props'

    export default defineComponent({
        name: "ChildFormSubmission",
        components: {
        },
        props,
        setup(p) {
           
            console.log('props: ', JSON.stringify(p));
            const queryParams = p.queryParameters;
            const dataAttributes = p.dataAttributes;
            return {
                queryParams,
				dataAttributes
            }
        }
    });
</script>

<template>
    <div>
        <h2>Child Form Submission</h2>
        <div>Query Params: {{JSON.stringify(queryParams)}}</div>
        <div>Data Attributes: {{JSON.stringify(dataAttributes)}}</div>
    </div>
</template>
