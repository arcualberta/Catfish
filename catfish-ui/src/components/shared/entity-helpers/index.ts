import { Guid } from "guid-typescript";
import { FieldData, Form, FormData, FormEntry } from '@/components/shared/form-models'
import { Entity } from "../../entity-editor/models";
import { EntityTemplate } from "../../entity-template-builder/models";
import { createFormData } from "../form-helpers";

/**
 * Returns the first occurrence of the FormData object that corresponds to the form identified by the formId from the input entity
 * 
 * @param entity
 * @param formId
 */
export const getFormData = (entity: Entity, formId: Guid): FormData => entity.data.filter(formData => formData.formId == formId)[0];

/**
 * Returns the first occurrence of the FieldData object that corresponds to the field identified by the fieldId in the 
 * first form data object of the form identified by the formId from the input entity
 * 
 * @param entity
 * @param formId
 * @param fieldId
 */
export const getFieldData = (entity: Entity, formId: Guid, fieldId: Guid): FieldData => getFormData(entity, formId)?.fieldData.filter(fieldData => fieldData.fieldId == fieldId)[0];

/**
 * Finds the form identified by the formId from the template, instantiates a FormData object for it, and add it
 * to the given entity.
 * 
 * @param entity
 * @param template
 * @param formId
 */
export const appendFormDataObject = (entity: Entity, template: EntityTemplate, formId: Guid) => {
    const form = template.forms!.filter(form => form.id == formId)[0];
    const formData = createFormData(form, "");
    formData.id = Guid.create().toString() as unknown as Guid;
    entity!.data.push(formData)

}

export const instantiateRequiredForm = (entity: Entity, template: EntityTemplate) => {
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.metadataForms!, template.forms!);
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.dataForms!, template.forms!);
}

const instantiateRequiredFormsFromArray = (entity: Entity, formEntries: FormEntry[], forms: Form[]) => {
    formEntries.filter(formEntry => formEntry.isPrimary).forEach(formEntry => {
        if (entity.data.filter(formData => formData.formId == formEntry.formId).length == 0) {
            appendFormDataObject
            const form = forms.filter(f => f.id === formEntry.formId)[0] as Form;
            const formData = createFormData(form, "");
            formData.id = Guid.create().toString() as unknown as Guid;
            entity.data.push(formData)
        }
    })
}