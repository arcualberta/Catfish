<script setup lang="ts">
    import { TransientMessageModel } from './models';
    import { watch } from "vue";

    const props = defineProps<{
        model: TransientMessageModel,
    }>();
    watch(() => props.model.transientMessage, async newMessage => {
        if (newMessage)
            setTimeout(() => {
                props.model.transientMessage = null;
            }, 2000)
    })
</script>

<template>
    <transition name="fade">
        <p v-if="model.transientMessage" :class="'alert alert-' + model.transientMessageClass">{{model.transientMessage}}</p>
    </transition>
</template>