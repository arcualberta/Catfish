<script setup lang="ts">
// This starter template is using Vue 3 <script setup> SFCs
// Check out https://vuejs.org/api/sfc-script-setup.html#script-setup

    import { getActivePinia } from 'pinia'
    import { useRoute } from 'vue-router'
    import { Guid } from 'guid-typescript'
    import {default as config} from "@/appsettings";

    import { FormBuilder } from '../components'
    import { useLoginStore } from '../components';

    const route = useRoute()
    const formId = route.params.id as unknown as Guid
    const apiRoot= config.dataRepositoryApiRoot + "/api/forms";

    const loginStore = useLoginStore();
    const jwtToken = loginStore.jwtToken;
</script>

<template>
    <FormBuilder  :api-root="apiRoot" :form-id="formId" :jwt-token="jwtToken" />
</template>
