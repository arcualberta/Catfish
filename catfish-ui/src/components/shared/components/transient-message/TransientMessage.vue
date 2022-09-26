<script setup lang="ts">
    import { TransientMessage} from './models';

    const props = defineProps<{
        model: TransientMessage,
    }>();
</script>

<template>
    <transition name="fade">
        <p v-if="model.transientMessage" :class="'alert alert-' + model.transientMessageClass">{{model.transientMessage}}</p>
    </transition>
</template>