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
    <h5>Children Fields -- form submission </h5>
    <div> {{JSON.stringify(model)}} -- {{model.type}}</div>
    <div v-for="(field) in model.fields" :key="field.id"> 
        <div>{{field.type}}</div>
        <FieldComponent :model="field" :modelData="compositeModelData?.filter(md=>md.fieldId == field.id)" />
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