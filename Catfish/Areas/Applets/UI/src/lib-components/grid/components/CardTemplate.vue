<script lang="ts">
	import { defineComponent, toRef, computed } from "vue";
    import { Card } from "../../shared/models/cmsModels";


    export default defineComponent({
		name: "CardTemplate",

        props: {
            model: Card
        },
		setup(props) {

            const model = toRef(props, 'model');

            return {
                model,
                imageUrl: computed(() => model.value?.cardImage?.media?.publicUrl?.substring(1)),
				imgStyle: computed(() => {
                    const url = model.value?.cardImage?.media?.publicUrl;
                    return { backgroundImage: "url(" + url?.substring(1) + ")" }
				}),
                popupImageUrl: computed(() => model.value?.modalImage?.media?.publicUrl),
                containerHeight: 250
            }
        }
    });
</script>

<template>
    <div class="col-md-4 m-5">
        <div v-bind:style="{backgroundImage:`url(${imageUrl})`, height:`${containerHeight}px`}" style="background-size:cover"> </div>

        {{JSON.stringify(model)}}
    </div>
</template>


