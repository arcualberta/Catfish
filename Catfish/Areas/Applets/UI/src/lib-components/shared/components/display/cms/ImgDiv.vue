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
                    if (this.root) {
                        const h = Math.round(this.root.clientWidth / this.aspectRatio);
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
			const root = ref<HTMLDivElement>();
            const resizeTimeout = ref<NodeJS.Timeout>();

            return {
				root,
				imageUrl: computed(() => image.value?.publicUrl?.replace(/^~+/g, '')), //NOTE: the REGEXP replaces any leading ~ characters
				aspectRatio: computed(() => image.value ? (image.value.width / image.value.height) : 1),
				resizeTimeout
            }
        }
    });
</script>

<template>
    <div ref="root"
         v-bind:style="{ backgroundImage: `url(${imageUrl})` }"
         style="background-size:cover"
         v-bind:id="image.id"
         class="cf-img-div"> </div>
</template>


