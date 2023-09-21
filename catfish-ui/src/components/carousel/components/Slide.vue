<script setup lang="ts">

</script>
<template>
    <div class="slide">
        <transition name="slide">
            <slot />
        </transition>
    </div>
</template>


<style lang="scss">
    @import "../styles/slide.scss";
</style>