
<script setup lang="ts">
    import { ref } from 'vue';
    import { Text, TextType } from '../../shared/form-models';
    import { useFormBuilderStore } from '../store';

    const props = defineProps<{ model: Text, textType: TextType, dispLang: boolean }>();
    const store = useFormBuilderStore();

</script>

<template>
    <div v-if="dispLang">
        <b-row v-if="textType===TextType.ShortAnswer">
            <b-col class="col-sm-1">
                <h6>{{model.lang}}: </h6>
            </b-col>
            <b-col class="col-sm-11">
                <b-form-input v-model="model.value"></b-form-input>
                <br />
            </b-col>
        </b-row>
        <b-row v-else-if="textType===TextType.Paragraph">
            <b-col class="col-sm-1">
                <h6>{{model.lang}}: </h6>
            </b-col>
            <b-col class="col-sm-11">
                <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
                <br />
            </b-col>
        </b-row>
    </div>
    <div v-else>
        <div v-if="textType===TextType.ShortAnswer">
            <b-form-input v-model="model.value"></b-form-input>
        </div>
        <div v-else-if="textType==='Paragraph'">
            <b-form-textarea v-model="model.value" rows="3" max-rows="6"></b-form-textarea>
        </div>
    </div>
</template>

