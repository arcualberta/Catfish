<script setup lang="ts">
    import Carousel from "./components/Carousel.vue"
    import Slide from "./components/Slide.vue"

    const slideImages = ["img-1", "img-2"];
    const getImageUrl = (name: string) => {
       
        return "../../src/assets/images/" + name + ".jpg";
    }
</script>
<template>
   <Carousel class="carousel" v-slot="{currentSlide}" :auto-play="true" :timeout-auto-play="10000" :navigation="true" :pagination="false">
       <Slide v-for="(slide, index) in slideImages" :key="index">
           <div class="slide-info" v-show="currentSlide === (index+1)">
              
               <img v-bind:src="getImageUrl(slide)" class="slide-img" />
              
           </div>
       </Slide>
   </Carousel>
</template>
<style lang="scss" scoped>
   @import "./styles/index.scss";
   @import "./styles/_carousel.scss";
</style> 