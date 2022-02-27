<script lang="ts">
    // References: 
    //  https://www.youtube.com/watch?v=wWKhKPN_Pmw
    //  https://github.com/johnkomarnicki/vue-3-dropzone

	import { defineComponent, PropType, computed} from 'vue'
    import { useStore } from 'vuex';

    import { AttachmentField } from '../../models/fieldContainer'
    import { FlattenedFormFiledMutations, FlattenedFormFiledState } from '../../store/form-submission-utils'
	import DropZone from './DropZone.vue'

    export default defineComponent({
        name: "AttachmentField",
        components: {
			DropZone
        },
        props: {
            model: {
                type: null as PropType<AttachmentField> | null,
                required: true
            },

        },
        setup(p) {
            const store = useStore();

            const drop = (e: any) => {
                Array.from(e.dataTransfer.files as FileList).forEach(file => {
                    store.commit(FlattenedFormFiledMutations.ADD_FILE, { id: p.model.id, val: file })
                });
            };

            const selectFiles = () => {
                const inputElement = document.getElementById(p.model.id.toString()) as HTMLInputElement;
				Array.from(inputElement?.files as FileList).forEach(file => {
					store.commit(FlattenedFormFiledMutations.ADD_FILE, { id: p.model.id, val: file })
				});
			};


            const selectedFiles = computed(() => {
                return (store.state as FlattenedFormFiledState).flattenedFileModels[p.model.id.toString()]
            });

            const selectedFileNames = computed(() => {
                return selectedFiles?.value?.map(file => file.name)
			});

            return {
                drop,
				selectFiles,
                selectedFiles,
				selectedFileNames
            };
        }
           
    });
</script>

<template>

    <div>{{selectedFileNames?.join(' | ')}}</div>
    <DropZone :id="model.id" @drop.prevent="drop" @change="selectFiles" />


    <!--<div>Attachment Field</div>
    <div>{{JSON.stringify(model)
        }}
    </div>-->
    <!--<div v-for="f in this.model.files" >-->
    <!--<input type="file" @change="handleFileUpload($event)" />-->
    <!--</div>-->
</template>

