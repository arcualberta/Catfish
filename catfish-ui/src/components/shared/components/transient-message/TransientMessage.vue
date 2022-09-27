<script setup lang="ts">
    import { TransientMessageModel } from './models';
    import { watch } from "vue";

    const props = defineProps<{
        model: TransientMessageModel,
    }>();
    watch(() => props.model.message, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                props.model.message = null;
            }, 2000)
    })
</script>

<template>
    <transition name="fade">
        <p v-if="model.message" :class="'alert alert-' + model.messageClass">{{model.message}}</p>
    </transition>
</template>