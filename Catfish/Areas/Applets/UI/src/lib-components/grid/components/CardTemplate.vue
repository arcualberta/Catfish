<script lang="ts">
	import { defineComponent, toRef, ref, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";


    export default defineComponent({
		name: "CardTemplate",

        props: {
            model: Card
        },
		setup(props) {

            const model = toRef(props, 'model');

            const hasCardImage = computed(() => model.value?.cardImage?.hasValue);
            const cardImageUrl = computed(() => model.value?.cardImage?.media?.publicUrl?.replace(/^~+/g, '')); //NOTE: the REGEXP replaces any leading ~ characters
            const cardImageaWidth = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.width : 1);
            const cardImageaHeight = computed(() => hasCardImage.value ? model.value?.cardImage?.media?.height : 1);
            const cardImageaAspectRatio = computed(() => cardImageaWidth.value && cardImageaHeight.value ? cardImageaWidth.value / cardImageaHeight.value : 0);
            const cardImgDiv = ref(null);
            const cardImageHeight = 250; //computed(() => cardImgDiv?.value?.cardImgDiv * cardImageaAspectRatio.value);

            const cardImageStyles = computed(() => {
                return {
                    backgroundImage: `url(${cardImageUrl.value})`,
					height: `${cardImageHeight}px`
                }
            });

            return {
                model,
				cardImgDiv,
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


