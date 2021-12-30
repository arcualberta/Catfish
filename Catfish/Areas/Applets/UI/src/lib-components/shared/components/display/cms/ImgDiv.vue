<script lang="ts">
    import { defineComponent, PropType, toRefs, ref, computed } from "vue";
    import { Media } from '../../../models/cmsModels'

    export default defineComponent({
		name: "ImgDiv",

        props: {
            image: {
				type: Object as PropType<Media>,
                required: true
            },
            debounceMs: {
                type: Number,
                required: false,
                default: 200
            }
        },
        methods: {
            resizeHandler() {
                this.setViewHeight(false);
            },
            setViewHeight(immediate: boolean) {
                if (this.resizeTimeout)
                    clearTimeout(this.resizeTimeout);

                const delay = immediate ? 0 : this.debounceMs;
                this.resizeTimeout = setTimeout(() => {
					if (this.image && this.root) {
                        const h = this.root.clientWidth * this.imageHeight / this.imageWidth;
                        this.root.style.height = `${h}px`;
                    }
				}, delay)
            }
        },
        created() {
			window.addEventListener("resize", this.resizeHandler);
			//console.log("Added resize event listener")
		},
		destroyed() {
			window.removeEventListener("resize", this.resizeHandler);
			//console.log("Removed resize event listener")
        },
        mounted() {
            this.setViewHeight(true);
		},
        setup(props) {
            //console.log("ImgDiv setup ...")

			const { image } = toRefs(props);
            
			const cardDivHeight = ref<number>(0);
			const root = ref<HTMLDivElement>();

			const resizeTimeout = ref<NodeJS.Timeout>();
            ////const setViewHeight = (immediate: boolean) => {
            ////    if (resizeTimeout.value)
            ////        clearTimeout(resizeTimeout.value);

            ////    const delay = immediate ? 0 : debounceMs.value;
            ////    resizeTimeout.value = setTimeout(() => {
            ////        if (image.value && root.value) {
            ////            const h = root.value.clientWidth * image.value.width / image.value.height;
            ////            root.value.style.height = `${h}px`;
            ////        }
            ////    }, delay)
            ////};


            return {
				root,
                image,
				imageUrl: computed(() => image.value?.publicUrl?.replace(/^~+/g, '')), //NOTE: the REGEXP replaces any leading ~ characters
				imageWidth: computed(() => image.value ? image.value.width : 1),
				imageHeight: computed(() => image.value ? image.value.height : 0),
                cardDivHeight,
				resizeTimeout
            }
        }
    });
</script>

<template>
    <div> Img DIV Setup</div>
    <div>Image URL: {{imageUrl}}</div>
    <div ref="root"
         v-bind:style="{ backgroundImage: `url(${imageUrl})`, height: `${cardDivHeight}px` }"
         style="background-size:cover"
         v-bind:id="image.id"> </div>
</template>


