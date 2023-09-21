<script setup lang="ts">
    import { ref } from 'vue';
    import Carousel from "./components/Carousel.vue"
    import Slide from "./components/Slide.vue"

    const props = defineProps<{
        navigation: boolean,
        pagination: boolean,
        autoPlay: boolean,
        timeoutAutoPlay: number,
        slides: string[]
    }>();

    const navigationEnabled = ref(props.navigation);
    const paginationEnabled = ref(props.pagination);
    const autoPlayEnabled = ref(props.autoPlay);
    const timeoutPlay = ref(props.timeoutAutoPlay);


    console.log(props.slides)
    const slideImages = props.slides;
    const getImageUrl = (name: string) => {
       
        return "../../src/assets/images/" + name + ".jpg";
    }
</script>
<template>
   <Carousel class="carousel" v-slot="{currentSlide}" :auto-play="autoPlayEnabled" :timeout-auto-play="timeoutPlay" :navigation="navigationEnabled" :pagination="paginationEnabled">
       <Slide v-for="(slide, index) in slideImages" :key="index">
           <div class="slide-info" v-show="currentSlide === (index+1)">
              
               <img v-bind:src="getImageUrl(slide)" class="slide-img" />
              
           </div>
       </Slide>
   </Carousel>
</template>
<style lang="scss" scoped>
   @import "./styles/index.scss";
</style> 