import { Guid } from "guid-typescript";
import { Field, FieldData, FieldEntry, FormTemplate, FormData } from '@/components/shared/form-models'
import { FormEntry } from '@/components/shared'
import { EntityData } from "../../entity-editor/models";
import { EntityTemplate } from "../../entity-template-builder/models";
import { createFormData } from "../form-helpers";
import { getConcatenatedValues } from "../form-helpers/textHelper";

/**
 * Returns the first occurrence of the FormData object that corresponds to the form identified by the formId from the input entity
 * 
 * @param entity
 * @param formId
 */
export const getFormData = (entity: EntityData, formId: Guid): FormData | undefined =>
    entity?.data.find(formData => formData.formId === formId);

/**
 * Returns the FieldData object that corresponds to the field identified by 
 * the fieldId from the given formData object
 * 
 * @param data
 * @param fieldId
 */
export const getFieldDataFromFormData = (data: FormData, fieldId: Guid): FieldData | undefined =>
    data?.fieldData.find(fieldData => fieldData.fieldId === fieldId);
    
/**
 * Returns the FieldData object that corresponds to the field identified by 
 * the fieldEntry.fieldId in the first form data object of the form identified by the fieldEntry.formId from the input entity
 * 
 * @param entity
 * @param fieldEntry
 */
export const getFieldData = (entity: EntityData, fieldEntry: FieldEntry): FieldData | undefined =>
    getFormData(entity, fieldEntry?.formId)?.fieldData.find(fieldData => fieldData.fieldId === fieldEntry?.fieldId);

/**
 * Returns the field specified by the fieldEntry.fieldId from the form with the given fieldEntry.formId from the input template.
 * 
 * @param template
 * @param fieldEntry
 */
export const getField = (template: EntityTemplate, fieldEntry: FieldEntry): Field | undefined =>
    template?.forms?.find(form => form?.id === fieldEntry.formId)?.fields.find(field => field.id === fieldEntry?.fieldId);

/**
 * Finds the form identified by the formId from the template, instantiates a FormData object for it, and add it
 * to the given entity.
 * 
 * @param entity
 * @param template
 * @param formId
 */
export const appendFormDataObject = (entity: EntityData, template: EntityTemplate, formId: Guid) => {
    const form = template.forms!.find(form => form.id == formId)!;
    const formData = createFormData(form, "");
    formData.id = Guid.create().toString() as unknown as Guid;
    entity!.data.push(formData)

}

/**
 * Instantiates FormData objects in the input "entity" for all required forms associated with its "template"
 * 
 * @param entity
 * @param template
 */
export const instantiateRequiredForms = (entity: EntityData, template: EntityTemplate) => {
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.metadataForms!, template.forms!);
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.dataForms!, template.forms!);
}

/**
 * This is a private function created to reduce code duplication in the above instantiateRequiredForms function.
 * @param entity
 * @param formEntries
 * @param forms
 */
const instantiateRequiredFormsFromArray = (entity: EntityData, formEntries: FormEntry[], forms: FormTemplate[]) => {
    formEntries.filter(formEntry => formEntry.isRequired).forEach(formEntry => {
        if (entity.data.filter(formData => formData.formId == formEntry.id).length == 0) {
            appendFormDataObject
            const form = forms.filter(f => f.id === formEntry.id)[0] as FormTemplate;
            const formData = createFormData(form, "");
            formData.id = Guid.create().toString() as unknown as Guid;
            entity.data.push(formData)
        }
    })
}

export const getConcatenatedTitle = (entity: EntityData, template:EntityTemplate, separator: string): string => {

    var titleField = template.entityTemplateSettings.titleField;
    var fieldData = entity.data.filter(dt=>dt.formId == titleField?.formId)[0]
                    .fieldData.filter(fd=>fd.fieldId == titleField?.fieldId)[0];
    return getConcatenatedValues(fieldData, separator);
  
}

export const getConcatenatedDescription = (entity: EntityData, template:EntityTemplate, separator: string): string => {

    var descriptionField = template.entityTemplateSettings.descriptionField;
    var descData = entity.data.filter(dt=>dt.formId == descriptionField?.formId)[0]
                    .fieldData.filter(fd=>fd.fieldId == descriptionField?.fieldId)[0];
    return getConcatenatedValues(descData, separator);
   
}

//export const getConcatenatedValues = (fieldData: FieldData, separator: string) : string => {
//    return "";
//}