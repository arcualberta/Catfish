<script setup lang="ts">
import { ref } from 'vue';
import { Field, FieldType } from '../../shared/form-models';
 
    const props = defineProps<{
        model: Field,
        elementId:string
        
    }>();
    const isAttachmentField = props.model.type === FieldType.AttachmentField ? true: false;
    const fieldElementId = props.elementId;
    const dropzoneFile=ref("");
    const selectFiles = () => {
            const inputElement = document.getElementById(fieldElementId) as HTMLInputElement;		
		};
    const active = ref(false);
    const toggleActive= ()=>{
        active.value = !active.value;
    }
</script>

<template>
   <div class="row" v-if="isAttachmentField">
        <div class="col-sm-2">
            <h6>File:</h6>
        </div>
        <div class="col-sm-10">
           <div class="dropzone" 
               @dragenter.prevent="toggleActive" 
               @dragleave.prevent="toggleActive"
               @dragover.prevent
               @drop.prevent="toggleActive"
               :class="{'active-dropzone': active}" >
             <div>Drag or Drop File</div>
              <div>OR</div>
            <label  :for="fieldElementId">Select File </label>
            <input type="file" :id="fieldElementId" /> 
           <!--  <DropZone :id="fieldElementId" @drop.prevent="drop" @change="selectFiles" /> -->
           </div>
        </div>
    
    </div>
</template>
<style scoped>
.dropzone{
        width: 300px;
        height: 100px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        row-gap: 6px;
        border: 2px dashed #41b883;
        background-color: #fff;
        transition: .3s ease all;
        font-size: medium;
    }
    .dropzone label{
        padding: 8px 12px;
        background-color: #41b883;
        color: #fff;
        transition: .3s ease all;
    }
    .dropzone input{
        display: none;
    }
    .active-dropzone{
        background-color:#41b883;
        color:#fff;
        border-color: #fff;
    }
    .active-dropzone label{
        background-color: #fff;
        color:#41b883;
    }
</style>