
<script lang="ts">
    import { defineComponent, PropType } from 'vue'
	import Editor from '@tinymce/tinymce-vue'
    import { Text } from '../../../models/textModels'

    export default defineComponent({
        name: "RichText",
        props: {
            model: {
                type: null as PropType<Text> | null,
                required: true
            },
            isRequired:
            {
                type: Boolean,
                required: false,
                default: false
            }
        },
        components: {
			Editor
        }
    });

    
</script>

<template>
    <div>
        <Editor v-model="model" placeholder="add multiple lines" required="{isRequired ? 'required' : ''}" />
        <!--<Editor   api-key="no-api-key"
       :init="{
      height: 500,
      menubar: false,
      plugins: [
        'advlist autolink lists link image charmap',
        'searchreplace visualblocks code fullscreen',
        'print preview anchor insertdatetime media',
        'paste code help wordcount table'
      ],
      toolbar:
        'undo redo | formatselect | bold italic | \
        alignleft aligncenter alignright | \
        bullist numlist outdent indent | help'
    }"  v-model="model" placeholder="add multiple lines" required="{isRequired ? 'required' : ''}" />-->
       
    </div>
</template>

<style scoped>
    /*.tox-notifications-container {
        display: none !important
    }*/
    .mce-notification {
        display: none !important;
    }
</style>

