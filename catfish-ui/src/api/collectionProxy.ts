import { default as config } from "@/appsettings";
import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";

import { EntityEntry } from "@/components/shared/models/listEntries";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
//import { WebClient } from "./webClient";

export class CollectionProxy{

    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/collections`);
    
    static async List (): Promise<EntityEntry[]> {
        return await this._crudProxy.List<EntityEntry>();
    }

    static async Get(id: Guid): Promise<EntityData> {
        return await this._crudProxy.Get<EntityData>(id);
    }

    static async Post(entityData: EntityData): Promise<boolean> {
       return await this._crudProxy.Post<EntityData>(entityData);
       
    }    

    static async Put(collection: EntityData): Promise<boolean> {
        return await this._crudProxy.Put(collection.id as Guid, collection); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await this._crudProxy.Delete(id)
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/collections`;
}