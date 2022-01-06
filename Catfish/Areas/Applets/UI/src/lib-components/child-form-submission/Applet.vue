<script lang="ts">
    import { defineComponent } from 'vue';
    import props, { QueryParameter } from '../shared/props'

    export default defineComponent({
        name: "ChildFormSubmission",
        components: {
        },
        props,
        setup(p) {
           
            console.log('props: ', JSON.stringify(p));
            const queryParams = p.queryParameters as QueryParameter;
            return {
               queryParams
            }
        }
    });
</script>

<template>
    <div>
        <h2>Child Form Submission</h2>
        
    </div>
</template>
