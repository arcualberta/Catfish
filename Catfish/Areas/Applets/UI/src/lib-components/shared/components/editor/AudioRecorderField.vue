<script lang="ts">
    import { defineComponent } from 'vue'
      

    export default defineComponent({
		name: "AudioRecorder",
        components: {
            
        },
        props: {
            model: {
           }
        },
        methods: {
		}
    });
</script>

<template>
    <div>Audio Recorder</div>
    <div>{{JSON.stringify(model)}}</div>
</template>
