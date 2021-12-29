<script lang="ts">
	import { defineComponent, PropType, toRefs/*, ref, onMounted*/, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";


    export default defineComponent({
		name: "CardTemplate",

        props: {
            model: {
                type: Object as PropType<Card>,
                required: true
            }
        },
		setup(props) {

            const { model } = toRefs(props);

            const hasCardImage = computed(() => model.value?.cardImage?.hasValue);
            const cardImageUrl = computed(() => model.value?.cardImage?.media?.publicUrl?.replace(/^~+/g, '')); //NOTE: the REGEXP replaces any leading ~ characters
            const cardImageaWidth = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.width : 1);
            const cardImageaHeight = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.height : 1);
            const cardImageaAspectRatio = computed(() => cardImageaWidth.value && cardImageaHeight.value ? cardImageaWidth.value / cardImageaHeight.value : 0);
            const cardImageHeight = 250; //computed(() => cardImgDiv?.value?.cardImgDiv * cardImageaAspectRatio.value);

   //         const cardImgDiv = ref<HTMLDivElement>();

			//onMounted(() => {
			//	// the DOM element will be assigned to the ref after initial render
			//	console.log(JSON.stringify(cardImgDiv.value)) // <div>This is a root element</div>
			//	console.log("Width: " , cardImgDiv.value?.clientWidth) // <div>This is a root element</div>
   //         })

            const cardImageStyles = computed(() => {
                return {
                    backgroundImage: `url(${cardImageUrl.value})`,
					height: `${cardImageHeight}px`
                }
            });

            return {
                model,
				//cardImgDiv,
                hasCardImage,
                cardImageStyles,
				cardImageaAspectRatio,
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


