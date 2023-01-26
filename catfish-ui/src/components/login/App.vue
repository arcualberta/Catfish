<script lang="ts" setup>
    import { onMounted, ref } from 'vue'
    import { Pinia } from 'pinia'
    import { useLoginStore } from './store'
    import { GoogleIdentityResult } from './models'

    const props = defineProps<{ //piniaInstance: Pinia, 
                             authorizationRoot: string}>();

    const authorizationStore = useLoginStore();

    onMounted(() => {
        authorizationStore.authorizationApiRoot = props.authorizationRoot;
    })

    const callback = (response: GoogleIdentityResult) => {
        // This callback will be triggered when the user selects or login to
        // their Google account from the popup
        authorizationStore.authorize(response.credential);
    }
</script>

<template>
    <h2>Login</h2>
    <br />
    <GoogleLogin :callback="callback" />
    <br />
</template>