<script lang="ts">
	import { defineComponent, PropType, toRefs, ref/*, onMounted*/, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";


    export default defineComponent({
		name: "CardTemplate",

        props: {
            model: {
                type: Object as PropType<Card>,
                required: true
            }
        },
        methods: {
            resizeHandler() {
                this.setViewHeight();
            },
            setViewHeight() {
                if (this.cardImgDiv) {
					const h = this.cardImgDiv.clientWidth * this.cardImageHeight / this.cardImageWidth;
                    this.cardImgDiv.style.height = `${h}px`;
				}
			}
        },
        created() {
			window.addEventListener("resize", this.resizeHandler);
			console.log("Added resize event listener")
		},
		destroyed() {
			window.removeEventListener("resize", this.resizeHandler);
			console.log("Removed resize event listener")
        },
        mounted() {
            this.setViewHeight();
		},
        setup(props) {

            const { model } = toRefs(props);

            const hasCardImage = computed(() => model.value?.cardImage?.hasValue);
            const cardImageUrl = computed(() => model.value?.cardImage?.media?.publicUrl?.replace(/^~+/g, '')); //NOTE: the REGEXP replaces any leading ~ characters
            const cardImageWidth = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.width : 1);
			const cardImageHeight = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.height : 0);

            const cardImgDiv = ref<HTMLDivElement>();
            const cardDivHeight = ref(0);

    //        onMounted(() => {
    //            this.setViewHeight();
				//// the DOM element will be assigned to the ref after initial render
				////console.log(JSON.stringify(cardImgDiv.value)) // <div>This is a root element</div>
				//console.log("Width: " , cardImgDiv.value?.clientWidth) // <div>This is a root element</div>
    //        })

            const cardImageStyles = computed(() => {
                return {
                    backgroundImage: `url(${cardImageUrl.value})`,
					height: `${cardDivHeight}px`
                }
            });


            return {
                model,
				cardImgDiv,
                hasCardImage,
                cardImageStyles,
				cardImageWidth,
                cardImageHeight,
                popupImageUrl: computed(() => model.value?.modalImage?.media?.publicUrl),
            }
        }
    });
</script>

<template>
    <div class="col-md-4 m-5">
        <div ref="cardImgDiv" v-if="hasCardImage" v-bind:style="cardImageStyles" style="background-size:cover" v-bind:id="model.cardImage.id"> </div>

        {{JSON.stringify(model)}}
    </div>
</template>


