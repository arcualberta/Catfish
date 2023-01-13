<script setup lang="ts">
import { Field as FieldModel } from '@/components/shared/form-models';
import {default as FieldComponent} from "./Field.vue"
import {default as ToolBar} from "./ToolBar.vue"
import { ref } from 'vue'
 import { useFormBuilderStore } from '../store'
import { storeToRefs } from 'pinia';
import { VueDraggableNext as draggable } from 'vue-draggable-next'
  
const store = useFormBuilderStore();
const {activeContainer} = storeToRefs(store);

const props = defineProps<{ model: FieldModel }>();

 const isOpen=ref(false);
    const fldIdx = ref(0);
    const isChildField=ref(true);
    const addField = (idx: number) => {
        activeContainer.value=props.model;
        isOpen.value= true;
        fldIdx.value = idx; //props.model.fields ? props.model.fields.length : 0;
    }
 const deleteField = (fldId: Guid) => {
        const idx = props.model.fields?.findIndex(fl => fl.id == fldId)
        props.model.fields?.splice(idx as number, 1)
    }
</script>



<template>

    <div class="lightGrayBorder compositeField"><h5>Children Fields </h5>
     <div class="insertFieldBtn fontSize2em"><font-awesome-icon icon="fa-solid fa-circle-plus" @click="addField(0)" class="fa-icon plus add-option" v-if="model.fields && model.fields.length == 0" />
     </div>
   
  
    <draggable class="dragArea list-group w-full" :list="model?.fields">

        <div v-for="(field, idx) in model?.fields" :key="field.id" class="form-field-border form-field">
           <div class="insertFieldBtn fontSize2em"> 
          
           <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addField(idx)" class="fa-icon plus"  v-if="idx==0" />
           </div>
           
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteField(field.id)" class="fa-icon field-delete" />
             <FieldComponent :model="field" />
            <div class="insertFieldBtn fontSize2em">
              <font-awesome-icon icon="fa-solid fa-circle-plus" @click="addField(idx + 1)" class="fa-icon plus" />
    
            </div>
        </div>
    </draggable>

<ToolBar :open="isOpen" @close="isOpen = !isOpen" :index="fldIdx" ></ToolBar>

 </div>
</template>



<style scoped>
.compositeField{
  padding: 10px;
  margin-bottom: 30px;
}
.lightGrayBorder{
  border: 1px solid lightgray;
  border-radius: 5px;
}
/* insert field button*/
.insertFieldBtn{
    text-align: center;
    width: 100%;
   }

/*.insertFieldBtn:before{
  content:" ";
  display: block;
  height: 1px;
  width: 45%;
  position: absolute;
  top: 56%;
  left: 30px;
  background: lightgrey;
}
.insertFieldBtn:after{
  content:" ";
  display: block;
  height: 1px;
  width: 45%;
  position: absolute;
  top: 56%;
  right: 30px;
  background: lightgrey;
}*/

.fontSize2em{
    font-size: 2rem;
}

</style>