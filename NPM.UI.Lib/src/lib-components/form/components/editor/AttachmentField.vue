<script lang="ts">
    import { defineComponent, PropType, computed} from "vue";
    import * as models from '../../models'
    import DropZone from './DropZone.vue'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    export default defineComponent({
        name: "AttachmentField",
        components: {
            DropZone
        },
        props: {
            model: {
                type: null as PropType<models.AttachmentField> | null,
                required: true
            },
        },
        setup(p) {
            const formStore = useFormSubmissionStore();
            let files = null as File[] | null;
            const drop = (e: any) => {
                Array.from(e.dataTransfer.files as FileList).forEach(file => {
                  
                    console.log("Drop: " + JSON.stringify(file))
                    files?.push(file)
                    formStore.updateFileReference(p.model?.id, file);
                });
               
            };

            const selectFiles = () => {
                const inputElement = document.getElementById(p.model?.id.toString()) as HTMLInputElement;
                Array.from(inputElement?.files as FileList).forEach(file => {
                 
                    formStore.updateFileReference(p.model?.id, file);
                });
            };

            const selectedFiles = computed(() => {
                //return (store.state as FlattenedFormFiledState).flattenedFileModels[p.model.id.toString()]
                console.log("get selected file: " + formStore.fileReferences)
                return formStore.fileReferences;
                
                
            });

            const selectedFileNames = computed(() => {
               
                return selectedFiles?.value?.map(f=>f.fileName)
            });
            return {
                drop,
                selectFiles,
                selectedFiles,
                selectedFileNames,
                files
            }

        }
    });
</script>


<template>
    Attachment
    <!--<div>{{selectedFileNames?.join(' | ')}}</div>-->
    <!--<DropZone :id="model.id" @drop.prevent="drop" @change="selectFiles" :key="model.id" />-->
    <input type="file" :id="model.id" @change="selectFiles" />

    <div>
        Selected Files: {{selectedFiles}}
    </div>
   
</template>
