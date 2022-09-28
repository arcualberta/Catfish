import { Guid } from "guid-typescript";
import { Field, FieldData, FieldEntry, Form, FormData, FormEntry } from '@/components/shared/form-models'
import { Entity } from "../../entity-editor/models";
import { EntityTemplate } from "../../entity-template-builder/models";
import { createFormData } from "../form-helpers";

/**
 * Returns the first occurrence of the FormData object that corresponds to the form identified by the formId from the input entity
 * 
 * @param entity
 * @param formId
 */
export const getFormData = (entity: Entity, formId: Guid): FormData =>
    entity?.data.filter(formData => formData.formId == formId)[0];

/**
 * Returns the FieldData objects that corresponds to the field identified by 
 * the fieldEntry.fieldId in the first form data object of the form identified by the fieldEntry.formId from the input entity
 * 
 * @param entity
 * @param fieldEntry
 */
export const getFieldData = (entity: Entity, fieldEntry: FieldEntry): FieldData[] =>
    getFormData(entity, fieldEntry?.formId)?.fieldData.filter(fieldData => fieldData.fieldId == fieldEntry?.fieldId);

/**
 * Returns the field specified by the fieldEntry.fieldId from the form with the given fieldEntry.formId from the input template.
 * 
 * @param template
 * @param fieldEntry
 */
export const getField = (template: EntityTemplate, fieldEntry: FieldEntry): Field | undefined =>
    template?.forms?.filter(form => form?.id === fieldEntry.formId)[0]?.fields.filter(field => field.id === fieldEntry?.fieldId)[0];

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

/**
 * Instantiates FormData objects in the input "entity" for all required forms associated with its "template"
 * 
 * @param entity
 * @param template
 */
export const instantiateRequiredForms = (entity: Entity, template: EntityTemplate) => {
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.metadataForms!, template.forms!);
    instantiateRequiredFormsFromArray(entity, template.entityTemplateSettings!.dataForms!, template.forms!);
}

/**
 * This is a private function created to reduce code duplication in the above instantiateRequiredForms function.
 * @param entity
 * @param formEntries
 * @param forms
 */
const instantiateRequiredFormsFromArray = (entity: Entity, formEntries: FormEntry[], forms: Form[]) => {
    formEntries.filter(formEntry => formEntry.isRequired).forEach(formEntry => {
        if (entity.data.filter(formData => formData.formId == formEntry.formId).length == 0) {
            appendFormDataObject
            const form = forms.filter(f => f.id === formEntry.formId)[0] as Form;
            const formData = createFormData(form, "");
            formData.id = Guid.create().toString() as unknown as Guid;
            entity.data.push(formData)
        }
    })
}

