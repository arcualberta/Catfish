<script lang="ts">
	import { defineComponent, PropType, toRefs/*, ref, onMounted*/, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";
    import ImgDiv from "../../shared/components/display/cms/ImgDiv.vue"

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
        setup(props) {

            const { model } = toRefs(props);

            return {
                model,
                cardImage: computed(() => model.value?.cardImage.media),
                popupImageUrl: computed(() => model.value?.modalImage?.media?.publicUrl),
                title: computed(() => model.value?.cardTitle.value),
                subTitle: computed(() => model.value?.cardSubTitle.value),
                hasModel: computed(() => model.value.hasAModal),
                modelImage: computed(() => model.value?.modalImage),
				modalTitle: computed(() => model.value?.modalTitle.value),
				modalSubTitle: computed(() => model.value?.modalSubTitle.value),
            }
        }
    });
</script>

<template>
    <div class="col-md-4 cf-card">
        <img-div :image="cardImage" :debounce-ms="250" class="img-div"/>
        <h2>{{title}}</h2>
        <h4>{{subTitle}}</h4>
    </div>
</template>

<style>
	.cf-card{
        text-align:center;
	}
</style>

