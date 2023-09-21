<script setup lang="ts">
    import { onMounted, ref } from 'vue'


    const props = defineProps<{
        navigation: boolean,
        pagination: boolean,
        autoPlay: boolean,
        timeoutAutoPlay: number
    }>();
    const currentSlide = ref(1);
    const getSlideCount = ref(0);
    const isAutoPlayEnabled = ref(props.autoPlay);
    console.log("auto play: " + props.autoPlay);
    const isPaginationEnabled = ref(props.pagination);
    const isNavigationEnabled = ref(props.navigation);
    const timeoutDuration = ref(props.timeoutAutoPlay === undefined ? 5000 : props.timeoutAutoPlay); // ref(props.timeoutAutoPlay); //10secs

    onMounted(() => {
        getSlideCount.value = document.querySelectorAll(".slide").length; //count total number of slide
        //console.log(getSlideCount.value);
    });


    //next slide toggle
    const nextSlide = () => {
        //make sure it's not over shoot
        if (currentSlide.value === getSlideCount.value) {
            currentSlide.value = 1;
            return;
        }

        currentSlide.value += 1;
    }

    //previous slide toggle

    const prevSlide = () => {
        if (currentSlide.value === 1) {
            //loop around
           currentSlide.value = getSlideCount.value;

            //or just keep it on the same slide
           // currentSlide.value = 1
            return;
        }

        currentSlide.value -= 1;
    }

    //go to slide
    const goToSlide = (index: number) => {
        currentSlide.value = index + 1; //add 1 because initial currentSlide= 1 not 0
    }


    //enable auto play
    const autoPlay = () => {
        setInterval(() => {
            nextSlide();
        }, timeoutDuration.value);
    }

    if (isAutoPlayEnabled.value)
        autoPlay();

 </script>
<template>
    <div class="carousel">

        <slot :currentSlide="currentSlide" />

        <div class="carousel-navigate" v-if="isNavigationEnabled">
            <!--<div class="toggle-page left"><font-awesome-icon icon="fa-chevron-left" class="fa-icon fa-chevron-left">  </font-awesome-icon></div>--> <!-- not found!!!!! -->
            <div class="toggle-page left">
                <font-awesome-icon icon="fa-icon fa-chevron-right" class="font-awesome-icon fa-icon fa-chevron-right" @click="prevSlide">  </font-awesome-icon>
            </div>
            <div class="toggle-page right">
                <font-awesome-icon icon="fa-icon fa-chevron-right" class="font-awesome-icon fa-icon fa-chevron-right" @click="nextSlide">  </font-awesome-icon>
            </div>

        </div>

        <!-- Pagination -->
        <div class="carousel-pagination" v-if="isPaginationEnabled">
            <span v-for="(slideNum, index) in getSlideCount" :key="index" class="page-number" :class="{active : index + 1 === currentSlide}" @click="goToSlide(index)"></span>
        </div>



    </div>
</template>
<style lang="scss" scoped>
    @import "../styles/_carousel.scss";
   
</style>