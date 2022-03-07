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
        let flds:ComponentField[] = [];
       // console.log("Item: " + JSON.stringify(state.item));
        for (let i = 0; i < components?.length; i++) {

            let frmTemplateId = components[i].formTemplateId;
            let fldId = components[i].fieldId;
           // console.log("form template Id : " + frmTemplateId + "field id: " + fldId);
           
            //let fld: Field;
            let fld = (state.item?.dataContainer?.$values?.filter(dc => dc.templateId === frmTemplateId)[0])?.fields.$values?.filter(fd => fd.id === fldId);
            //console.log("the field: " + fld.n);
            let comField: ComponentField = { component: components[i], field: fld as unknown as Field };
           

            flds.push(comField);
        }
        //console.log("fields:")
      //  console.log(JSON.stringify(flds));
        return flds;
    }
}
