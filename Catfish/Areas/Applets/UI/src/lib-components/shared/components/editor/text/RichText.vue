
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
        <Editor 
                apiKey="0ohehg73era56wydy5kyws6ouf25550ogy2sifi1j41hk65l" 
                v-model="model" 
                placeholder="add multiple lines" 
                required="{isRequired ? 'required' : ''}" 
                />

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
	.tox-tinymce-aux, .mce-notification {
		display: none !important;
	}
</style>

