<script>
    //References:
    //  https://github.com/johnkomarnicki/vue-3-dropzone
    //  https://www.youtube.com/watch?v=wWKhKPN_Pmw
    //

	import { ref, defineComponent } from "vue";

	export default defineComponent({
        name: "DropZone",
        props: {
			id: {
                required: true,
                type: String
			},
        },
        setup() {
            const active = ref(false);

            const toggleActive = () => {
                active.value = !active.value;
            };

            return { active, toggleActive };
        },
    });
</script>


<template>
    <div @dragenter.prevent="toggleActive"
         @dragleave.prevent="toggleActive"
         @dragover.prevent
         @drop.prevent="toggleActive"
         :class="{ 'active-dropzone': active }"
         class="dropzone">
        <span>Drag and Drop File(s)</span>
        <span>OR</span>
        <label :for="id">Select File(s)</label>
        <input type="file" :id="id" class="dropzoneFile" multiple/>
    </div>
</template>


<style scoped>
    .dropzone {
        width: 400px;
        height: 200px;
        margin-top:20px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        row-gap: 16px;
        border: 2px dashed #41b883;
        background-color: #fff;
        transition: 0.3s ease all;
    }

        .dropzone label {
            padding: 8px 12px;
            color: #fff;
            background-color: #41b883;
            transition: 0.3s ease all;
        }

        .dropzone input {
            display: none;
        }

    .active-dropzone {
        color: #fff;
        border-color: #fff;
        background-color: #41b883;
    }

        .active-dropzone label {
            background-color: #fff;
            color: #41b883;
        }
</style>
