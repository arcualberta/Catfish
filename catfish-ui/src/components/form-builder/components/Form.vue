
<script setup lang="ts">
    import { Guid } from "guid-typescript";
    import { VueDraggableNext as draggable } from 'vue-draggable-next'
    import { getFieldTitle } from '../../shared/form-helpers';
    import { default as Field } from './Field.vue'
    import { default as TextCollection } from './TextCollection.vue'
    import { FormTemplate } from "@/components/shared/form-models/formTemplate";

    import {default as ToolBar} from "./ToolBar.vue"
    import { ref } from "vue";
    /* import the fontawesome core */
    import { library } from '@fortawesome/fontawesome-svg-core'

    /* import font awesome icon component */
    import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

    /* import specific icons */
    import * as faIcons from '@fortawesome/free-solid-svg-icons'
    /* add icons to the library */
    library.add(faIcons.faCircleCheck)
    library.add(faIcons.faCircleXmark)
    library.add(faIcons.faPenToSquare)
    library.add(faIcons.faCirclePlus)
    library.add(faIcons.faQuestionCircle)
    library.add(faIcons.faThList)
    library.add(faIcons.faArrowLeft)

    

    const props = defineProps<{ model: FormTemplate }>();
    const deleteField = (optId: Guid) => {
        const idx = props.model.fields?.findIndex(opt => opt.id == optId)
        props.model.fields?.splice(idx as number, 1)
    }
    // for the toolbar popup
    const isOpen=ref(false);
    const fldIdx = ref(0)
</script>

<template>
    <div>
        <h4>Form properties</h4>
    </div>
    <div class="form-field-border">
        <b-row>
            <b-col class="col-sm-2">
                <h6 >Name:</h6>
            </b-col>
            <b-col class="col-sm-10">
                <b-form-input v-model="model.name"></b-form-input>
            </b-col>
        </b-row>
        <br />
        <b-row>
            <b-col class="col-sm-2">
                <h6 >Description:</h6>
            </b-col>
            <b-col class="col-sm-10">
                <b-form-textarea v-model="model.description" rows="3" max-rows="6"></b-form-textarea>
            </b-col>
        </b-row>
    </div>

    <h3>Fields</h3>
    <div class="insertFieldBtn fontSize2em">
      <font-awesome-icon icon="fa-solid fa-circle-plus" @click="isOpen=true" class="fa-icon plus"  v-if="model.fields.length == 0" />
    </div>
    <draggable class="dragArea list-group w-full" :list="model?.fields">

        <div v-for="(field, idx) in model?.fields" :key="field.id" class="form-field-border form-field">
           <div class="insertFieldBtn fontSize2em"> 
          
           <font-awesome-icon icon="fa-solid fa-circle-plus" @click="isOpen=true;fldIdx=idx" class="fa-icon plus"  v-if="idx==0" />
           </div>
           
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="deleteField(field.id)" class="fa-icon field-delete" />
            <Field :model="field" />
            <div class="insertFieldBtn fontSize2em">
              <font-awesome-icon icon="fa-solid fa-circle-plus" @click="isOpen=true;fldIdx=idx+1" class="fa-icon plus" />
    
            </div>
        </div>
    </draggable>

    <ToolBar :open="isOpen" @close="isOpen = !isOpen" :index="fldIdx"></ToolBar>
</template>

<style scoped>

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
