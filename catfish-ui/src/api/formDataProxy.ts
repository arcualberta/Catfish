import { default as config } from "@/appsettings";
//import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
//import { EntityTemplate } from "@/components/entity-template-builder/models";
import { FormData } from "@/components/shared/form-models";

//import { FormEntry } from "@/components/shared/models/listEntries";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
//import { WebClient } from "./webClient";

export class FormDataProxy{

    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/form-submissions`);
    
    static async List (): Promise<Guid[]> {
        return await this._crudProxy.List<Guid>();
    }

    static async Get(id: Guid): Promise<FormData> {
        return await this._crudProxy.Get<FormData>(id);
    }

    static async Post<FormData extends ObjectId>(formData: FormData): Promise<boolean> {
       return await this._crudProxy.Post<FormData>(formData);
       
    }    

    static async Put(formTemplate: FormData): Promise<boolean> {
        return await this._crudProxy.Put(formTemplate.id as Guid, formTemplate); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await this._crudProxy.Delete(id)
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/form-submissions`;
}