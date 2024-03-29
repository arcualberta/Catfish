<script setup lang="ts">
    import { computed, onMounted, toRef, watch } from "vue";
    import { Pinia } from 'pinia'
    import { Guid } from "guid-typescript";
    import { TransientMessageModel } from '../shared/components/transient-message/models'
    import { useFormBuilderStore } from './store';
    import { Field } from '../shared/form-models'
    import { createTextCollection, isOptionField, isTextInputField, createOption } from '../shared/form-helpers'
    import { FieldType } from '../shared/form-models';
    import { AppletAttribute } from '../shared/props'
    import { default as TransientMessage } from '../shared/components/transient-message/TransientMessage.vue'
    import '../../style.css'

    import { default as Form } from './components/Form.vue';
    import { useRoute } from "vue-router";
import router from "@/router";

    const props = defineProps<{
        dataAttributes?: AppletAttribute | null,
        queryParameters?: AppletAttribute | null,
       // piniaInstance: Pinia,
       // repositoryRoot: string,
        formId?: Guid,
        apiRoot: string | null,
        jwtToken: string | null
    }>();

    const store = useFormBuilderStore();
    const route = useRoute()
   
    const formId = props.formId? props.formId : route.params.id as unknown as Guid;
    if(props.apiRoot){
        //console.log("api root from props: " + props.apiRoot);
        store.setApiRoot(props.apiRoot);
    }
    //const transientMessage = computed(() => store.transientMessageModel);
    //const _dataAttributes = toRef(props, 'dataAttributes')
    //const userJwtToken = _dataAttributes && _dataAttributes?.value? (_dataAttributes.value["UserJwtToken"] as string) : null;
    if(props.jwtToken)
         store.jwtToken=props.jwtToken as string;
         
    if (formId)
        store.loadForm(formId)

    
    const saveForm = () => {
        store.saveForm()
        .then(status => {
            if(status){
                const newRoute = `${router.currentRoute.value.path}/${store.form?.id}`;
                router.push(newRoute)
            }
        })
    }

    const disabled = computed(() => store.form ? false : true)

    const newField = (fieldType: FieldType) => {

        const field = {
            id: Guid.create().toString() as unknown as Guid,
            title: createTextCollection(store.lang),
            description: createTextCollection(store.lang),
            type: fieldType,
        } as Field;

        //TODO: Restrict the following isMultiValued property only for monolingual and multilingual fields. We should leave
        //it undefined for other field types.
        if (isTextInputField(field)) {
            field.isMultiValued = false;
            field.isRequired = false;
            field.allowCustomOptionValues = false;
        }
        if (isOptionField(field)) {
            field.options = []
        }
        store.form!.fields.push(field);
    }

    onMounted(()=>{
        store.createNewForm();
    });

</script>
<style scoped src="./styles.css"></style>

<template>
   <div class="control">
     
        <button type="button" class="btn btn-success" :disabled="disabled" @click="saveForm">Save</button>
    </div>
     <TransientMessage :model="store.transientMessageModel"></TransientMessage>
   
   
    <Form v-if="store.form" :model="store.form" />
    <!--
    <div class="toolbar">
        <button :disabled="disabled" @click="newField(FieldType.ShortAnswer)">+ Short Answer</button>
        <button :disabled="disabled" @click="newField(FieldType.Paragraph)">+ Paragraph</button>
        <button :disabled="disabled" @click="newField(FieldType.RichText)">+ Rich Text</button>
        <button :disabled="disabled" @click="newField(FieldType.Date)">+ Date</button>
        <button :disabled="disabled" @click="newField(FieldType.DateTime)">+ Date/Time</button>
        <button :disabled="disabled" @click="newField(FieldType.Decimal)">+ Decimal</button>
        <button :disabled="disabled" @click="newField(FieldType.Integer)">+ Integer</button>
        <button :disabled="disabled" @click="newField(FieldType.Email)">+ Email</button>
        <button :disabled="disabled" @click="newField(FieldType.Checkboxes)">+ Checkboxes</button>
        <button :disabled="disabled" @click="newField(FieldType.DataList)">+ Data List</button>
        <button :disabled="disabled" @click="newField(FieldType.RadioButtons)">+ Radio Buttons</button>
        <button :disabled="disabled" @click="newField(FieldType.DropDown)">+ Drop Down</button>
        <button :disabled="disabled" @click="newField(FieldType.InfoSection)">+ Info Section</button>
        <button :disabled="disabled" @click="newField(FieldType.AttachmentField)">+ Attachment Field</button>
    </div>
    -->
    <hr />

    <div class="alert alert-info">
        {{ store.form }}
    </div>
   

</template>

