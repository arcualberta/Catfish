import { default as config } from "@/appsettings";
import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";

import { EntityEntry } from "@/components/shared/models/listEntries";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
//import { WebClient } from "./webClient";

export class ItemProxy{

    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/items`);
    
    static async List (): Promise<EntityEntry[]> {
        return await this._crudProxy.List<EntityEntry>();
    }

    static async Get(id: Guid): Promise<EntityData> {
        return await this._crudProxy.Get<EntityData>(id);
    }

    static async Post<EntityData extends ObjectId>(item: EntityData): Promise<boolean> {
       return await this._crudProxy.Post<EntityData>(item);
       
    }    

    static async Put(item: EntityData): Promise<boolean> {
        return await this._crudProxy.Put(item.id as Guid, item); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await this._crudProxy.Delete(id)
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/items`;
}