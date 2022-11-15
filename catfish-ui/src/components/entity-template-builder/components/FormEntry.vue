<script setup lang="ts">
    import { Pinia } from 'pinia';
    import { computed } from 'vue';
    import { FormEntry } from '../../shared';
    import { useEntityTemplateBuilderStore } from '../store';
    import { SelectableOption } from '@/components/shared/components/form-field-selection-dropdown/models'
    
import { library } from '@fortawesome/fontawesome-svg-core'
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

    
    const props = defineProps<{
        model: FormEntry
    }>();

    const store = useEntityTemplateBuilderStore();

</script>

<template>
   
    <b-row class="mb-2">
        <b-col class="col-sm-11">
            <b-row>
                <b-col class="col-sm-2">
                    <h6>Name :</h6>
                </b-col>
                <b-col class="col-sm-10">
                    <b-form-input v-model="model.name"></b-form-input>
                </b-col>
            </b-row>
            <br />
            <b-row>
                <b-col class="col-sm-2">
                    <label :for="model.id.toString()">Form:</label>
                </b-col>
                <b-col class="col-sm-10">
                    <select v-model="model.id" :name="model.id.toString()" class="form-select">
                        <option v-for="entry in store.formEntries" :key="entry.id.toString()" :value="entry.id">{{entry.name}}</option>
                    </select>
                </b-col>
            </b-row>
            <div>
                <input type="checkbox" v-model="model.isRequired" /> Is Required?
            </div>
        </b-col>
        <b-col class="col-sm-1">
            <font-awesome-icon icon="fa-solid fa-circle-xmark" @click="store.deleteFormEntry(model.id)" class="fa-icon delete" />
        </b-col>
    </b-row>
</template>

<style scoped src="../style.css"></style>
