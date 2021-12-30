<script lang="ts">
	import { defineComponent, PropType, toRefs/*, ref, onMounted*/, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";
    import ImgDiv from "../../shared/components/display/cms/ImgDiv.vue"
	//import { useDebouncedRef } from '../../shared/functions/useDebouncedRef'

    export default defineComponent({
        name: "CardTemplate",
        components: {
			ImgDiv
		},
        props: {
            model: {
                type: Object as PropType<Card>,
                required: true
            }
        },
  //      methods: {
  //          resizeHandler() {
  //              this.setViewHeight(false);
  //          },
  //          setViewHeight(immediate: boolean) {
  //              if (this.resizeTimeout)
  //                  clearTimeout(this.resizeTimeout);

  //              const delay = immediate ? 0 : 250;
  //              this.resizeTimeout = setTimeout(() => {
  //                  if (this.cardImgDiv) {
  //                      const h = this.cardImgDiv.clientWidth * this.cardImageHeight / this.cardImageWidth;
  //                      this.cardImgDiv.style.height = `${h}px`;
  //                  }
		//		}, delay)

  //              //const h = this.cardImgDiv.clientWidth * this.cardImageHeight / this.cardImageWidth;
  //              //this.cardDivHeight = this.cardImgDiv.clientWidth * this.cardImageHeight / this.cardImageWidth;
  //          }
  //      },
  //      created() {
		//	window.addEventListener("resize", this.resizeHandler);
		//	console.log("Added resize event listener")
		//},
		//destroyed() {
		//	window.removeEventListener("resize", this.resizeHandler);
		//	console.log("Removed resize event listener")
  //      },
  //      mounted() {
  //          this.setViewHeight(true);
		//},
        setup(props) {

            const { model } = toRefs(props);

   //         const hasCardImage = computed(() => model.value?.cardImage?.hasValue);
   //         const cardImageUrl = computed(() => model.value?.cardImage?.media?.publicUrl?.replace(/^~+/g, '')); //NOTE: the REGEXP replaces any leading ~ characters
   //         const cardImageWidth = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.width : 1);
			//const cardImageHeight = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.height : 0);

   //         const cardImgDiv = ref<HTMLDivElement>();

   //         const resizeTimeout = ref<NodeJS.Timeout>();
			//const cardDivHeight = ref<number>(0);


   //         const cardImageStyles = computed(() => {
   //             return {
   //                 backgroundImage: `url(${cardImageUrl.value})`,
			//		height: `${cardDivHeight}px`
   //             }
   //         });


            return {
                model,
                cardImage: computed(() => model.value?.cardImage.media),
				//cardImgDiv,
    //            hasCardImage,
    //            cardImageStyles,
				//cardImageWidth,
    //            cardImageHeight,
    //            cardDivHeight,
				//resizeTimeout,
                popupImageUrl: computed(() => model.value?.modalImage?.media?.publicUrl),
            }
        }
    });
</script>

<template>
    <div class="col-md-4 m-5">
        <!--<div ref="cardImgDiv" v-if="hasCardImage" v-bind:style="cardImageStyles" style="background-size:cover" v-bind:id="model.cardImage.id"> </div>-->
        <img-div :image="cardImage" :debounce-ms=250 />
        {{JSON.stringify(model)}}
    </div>
</template>


