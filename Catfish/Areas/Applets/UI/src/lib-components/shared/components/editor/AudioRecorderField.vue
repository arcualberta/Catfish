<script lang="ts">
    import { defineComponent, PropType } from 'vue'
    import { AudioRecorderField } from '../../models/fieldContainer'

    export default defineComponent({
		name: "AudioRecorder",
        components: {
            
        },
        props: {
            model: {
                type: Object as PropType<AudioRecorderField> | null,
                required: true
           }
        },
        methods: {
		}
    });
</script>

<template>
    <div>Audio Recorder</div>
    <!--<div>{{JSON.stringify(model)}}</div>-->
</template>
