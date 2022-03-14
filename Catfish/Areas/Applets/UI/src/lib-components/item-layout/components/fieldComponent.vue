<script lang="ts">
    import { defineComponent, PropType, computed, ref } from 'vue'
    import { useStore } from 'vuex';
    import dayjs from "dayjs";
    import { ComponentField } from "../models/componentField"
    import {/* Field,*/  OptionsField, OptionsFieldMethods/*, AttachmentField*/ } from "../../shared/models/fieldContainer"
    import ImageCarousel from "./imageCarousel.vue"
    import ImageGallery from "./imageGallery.vue"
  
   
   export default defineComponent({
            name: "FieldComponent",
       components: {
           ImageCarousel,
           ImageGallery
        },
       props: {
           model: {
               type: null as PropType<ComponentField> | null,
               required: true
           },
       },
       setup(p) {

          
           const store = useStore();
           let fileUrl = "";
           if (p.model.field.$type.includes("AttachmentField")) {
               const itemId = ref(store.state.item.id);

               const dataItemId = ref(store.getters.dataItemId(p.model.component.formTemplateId));
               const fieldId = ref(p.model.component.fieldId);
               //const fileName = ref((p.model.field as AttachmentField).files.$values[0].fileName); //ref((p.model.field as AttachmentField)
               fileUrl = window.location.origin + '/api/items/' + itemId.value + '/' + dataItemId.value + '/' + fieldId.value + '/';// + fileName.value;
              // console.log("url: " + fileUrl);
           }
           let displayImagesMode = "";
           if (p.model.component.displayImagesMode) {
               displayImagesMode = p.model.component.displayImagesMode == 'gallery' ? 'gallery' : 'carousel';
           }
          
            return {
                htmlWrapperTag: computed(() => p.model.component.type?.length > 0 ? p.model.component.type : "div"),
				componentType: computed(() => p.model.component.$type.split(',')[0]),
                field: computed(() => p.model.field),
                fieldType: computed(() => p.model.field.$type?.split(',')[0]),
               //fieldType: computed(() => p.model.field.$type)
                fileUrl,
                displayImagesMode
            }
       },
       methods: {
           formatDate(dateString: string) {
               const date = dayjs(dateString);
               return date.format('MMM DD, YYYY');
           },
           formatToDecimal: (value: number, decimalPlaces: number) => {
               return Number(value).toFixed(decimalPlaces);
           },
           getSelectedFieldLabels(field: OptionsField) {
               return OptionsFieldMethods.getSelectedFieldLabels(field.options.$values);
           },
           
       }
       
    });
</script>

<template>

   
    <div class="fieldType">
       
        <hr />
        <component :is="htmlWrapperTag" v-if="componentType === 'Catfish.Areas.Applets.Models.Blocks.ItemLayout.StaticText'"> {{model.component.content}} </component>
        <div v-else>
            <div v-if="fieldType === 'Catfish.Core.Models.Contents.Fields.TextArea' || fieldType === 'Catfish.Core.Models.Contents.Fields.TextField'">
                <div v-for="multilingualValue in field.values.$values">
                    <component :is="htmlWrapperTag" v-for="valueInOneLanguage in multilingualValue.values.$values">
                        {{valueInOneLanguage.value}}
                    </component>
                </div>
            </div>
            <div v-else-if="fieldType.includes('Catfish.Core.Models.Contents.Fields.EmailField') || fieldType === 'Catfish.Core.Models.Contents.Fields.MonolingualTextField' || fieldType === 'Catfish.Core.Models.Contents.Fields.IntegerField'">
                <component :is="htmlWrapperTag" v-for="val in field.values.$values">
                    {{val.value}}
                </component>
            </div>
            <div v-else-if="fieldType === 'Catfish.Core.Models.Contents.Fields.DecimalField'">
                <component :is="htmlWrapperTag" v-for="val in field.values.$values">
                    {{formatToDecimal(val.value, 2)}}
                </component>
            </div>
            <div v-else-if="fieldType === 'Catfish.Core.Models.Contents.Fields.DateField'">
                <component :is="htmlWrapperTag" v-for="val in field.values.$values">
                    {{formatDate(val.value)}}
                </component>
            </div>
            <div v-else-if="fieldType === 'Catfish.Core.Models.Contents.Fields.OptionsField' || fieldType === 'Catfish.Core.Models.Contents.Fields.CheckboxField' || fieldType === 'Catfish.Core.Models.Contents.Fields.RadioField' || fieldType === 'Catfish.Core.Models.Contents.Fields.SelectField'">
                <component :is="htmlWrapperTag">
                    {{getSelectedFieldLabels(field)}}
                </component>

            </div>
            <div v-else-if="fieldType.includes('Catfish.Core.Models.Contents.Fields.AttachmentField') || fieldType === 'Catfish.Core.Models.Contents.Fields.AudioRecorderField'">
               
                <ImageCarousel v-if="displayImagesMode === 'carousel'" :model="field" :fileUrl="fileUrl" />
                <ImageGallery v-else-if="displayImagesMode === 'gallery'" :model="field" :fileUrl="fileUrl" />

                <div v-else v-for="val in field.files.$values">

                    <a v-if="val.contentType.includes('pdf')" :href="fileUrl + val.fileName">{{val.originalFileName}}</a>
                    <img v-if="val.contentType.includes('image')" :src="fileUrl + val.fileName" />
                    <audio controls v-if="val.contentType.includes('audio')">
                        <source :src="fileUrl + val.fileName" :type="val.contentType">

                        Your browser does not support the audio element.
                    </audio>

                </div>
            </div>
        
        </div>
        <br />
        <hr />

    </div>
</template>

