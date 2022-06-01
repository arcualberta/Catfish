<script lang="ts">
    import { defineComponent, PropType, computed} from "vue";
    import * as models from '../../models'
    import DropZone from './DropZone.vue'
    import { useFormSubmissionStore } from '../../store/FormSubmissionStore'
    import { Guid } from 'guid-typescript';

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

                //file has been saved -- remove from input element value
                
            };

            const selectedFiles = computed(() => {
                //return (store.state as FlattenedFormFiledState).flattenedFileModels[p.model.id.toString()]
              //  console.log("get selected file: " + formStore.fileReferences)
                return formStore.fileReferences;
                
                
            });

            const selectedFileNames = computed(() => {
               
                return selectedFiles?.value?.map(f=>f.fileName)
            });

            const removeFile = (fieldId:Guid, fileId:Guid) => {
                formStore.deleteFileReference(fieldId, fileId);

            };
            return {
                drop,
                selectFiles,
                selectedFiles,
                selectedFileNames,
                removeFile
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
        Selected Files:
        <div v-for="fileRef in selectedFiles">
            <span>{{fileRef.fileName}}</span><span style="margin-left:30px; color: red" @click="removeFile(model.id, fileRef.id)">x</span>
        </div>
    </div>
   
</template>
