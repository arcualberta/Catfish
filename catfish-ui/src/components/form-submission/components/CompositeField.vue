<script setup lang="ts">
import { Field as FieldModel, FieldData } from '@/components/shared/form-models';
import {default as FieldComponent} from "./Field.vue"
import {computed} from "vue"

const props = defineProps<{
         model: FieldModel,
         modelData?: FieldData | null,
    }>();

    
const compositeModelData = computed(()=> props.modelData?.compositeFieldData);


</script>

<template>

    <div class="lightGrayBorder compositeField">
   
    <div v-for="(field, idx) in model.fields" :key="field.id"> 
      
        <FieldComponent :model="field" :modelData="compositeModelData![idx]" />
       
       
    </div>

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


.fontSize2em{
    font-size: 2rem;
}

</style>