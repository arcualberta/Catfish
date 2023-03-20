import { default as config } from "@/appsettings";
import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { FormTemplate } from "@/components/shared/form-models";

import { FormEntry } from "@/components/shared/models/listEntries";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
//import { WebClient } from "./webClient";

export class FormProxy{

    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/forms`);
    
    static async List (): Promise<FormEntry[]> {
        return await this._crudProxy.List<FormEntry>();
    }

    static async Get(id: Guid): Promise<FormTemplate> {
        return await this._crudProxy.Get<FormTemplate>(id);
    }

    static async Post<FormTemplate extends ObjectId>(formData: FormTemplate): Promise<boolean> {
       return await this._crudProxy.Post<FormTemplate>(formData);
       
    }    

    static async Put(formTemplate: FormTemplate): Promise<boolean> {
        return await this._crudProxy.Put(formTemplate.id as Guid, formTemplate); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await this._crudProxy.Delete(id)
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/forms`;
}