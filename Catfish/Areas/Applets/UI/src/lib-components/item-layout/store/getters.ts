/// <reference path="mutations.ts" />
import { Guid } from 'guid-typescript';
import { GetterTree } from 'vuex';
import { FieldLayout } from '../models/fieldLayout';
import { State } from './state';
import { ComponentField } from "../models/componentField"
import {Field } from "../../shared/models/fieldContainer"

export const getters: GetterTree<State, State> = {
   
    dataItem: (state) => (itemTemplateId: Guid) => {
        return (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === itemTemplateId)[0]);
    },
    field: (state) => (itemTemplateId: Guid, fieldId: Guid) => {

        return (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === itemTemplateId)[0])?.fields.$values?.filter(fd => fd.id === fieldId);
    },
    fields: (state) => (components: FieldLayout[]) => {
        const fields: ComponentField[] = [];
        for (let i = 0; i < components?.length; i++) {
            //console.log("component count: " + components?.length);
           // console.log("fieldId: " + components[i].fieldId);
            const frmTemplateId = components[i].formTemplateId;
            const fldId = components[i].fieldId;
            if (typeof fldId != 'undefined' && fldId) {
                const field = (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === frmTemplateId)[0])?.fields.$values?.filter(fd => fd.id === fldId)[0];

                const comField: ComponentField = { component: components[i], field: field as Field };
                fields.push(comField);
            }
           // else {
            //    const comField: ComponentField = {
            //        component: components[i], field: {} as Field};
            //    fields.push(comField);
            //}

           
        }
       // console.log("fields: " + JSON.stringify(fields));
        return fields;
    },
    dataItemId: (state) => (formTemplateId: Guid) => {
        return (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === formTemplateId)[0])?.id;
    }
}
