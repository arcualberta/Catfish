import { default as config } from "@/appsettings";
import { TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { ReturnVoid } from "@/components/form-submission/__VLS_types";
import { eState } from "@/components/shared/constants";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
import { WebClient } from "./webClient";

export class EntityTemplateProxy{

    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/entity-templates`);
    
    static async List (): Promise<TemplateEntry[]> {
        return await EntityTemplateProxy._crudProxy.List<TemplateEntry>();
    }

    static async Get(id: Guid): Promise<EntityTemplate> {
        return await EntityTemplateProxy._crudProxy.Get<EntityTemplate>(id);
    }

    static async Post<EntityTemplate extends ObjectId>(entityTemplate: EntityTemplate): Promise<boolean> {
       return await this._crudProxy.Post<EntityTemplate>(entityTemplate);
        /* let newIdCreated = false
        try{
            if(!entityTemplate.id || entityTemplate.id == Guid.parse(Guid.EMPTY)) {
                entityTemplate.id = Guid.create().toString() as unknown as Guid
                newIdCreated = true
            }
            var response = await WebClient.postJson(`${this.getApiRoot()}`, entityTemplate)
            return true;
        }
        catch(e){
            if(newIdCreated){
                entityTemplate.id = Guid.parse(Guid.EMPTY)
            }
            throw e;
        }*/
    }    

    static async Put(entityTemplate: EntityTemplate): Promise<boolean> {
        return await EntityTemplateProxy._crudProxy.Put(entityTemplate.id as Guid, entityTemplate); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await EntityTemplateProxy._crudProxy.Delete(id)
    }
    static async ChangeState(id: Guid, newState: eState): Promise<boolean>{
        var response = await WebClient.postJson(`${this.getApiRoot()}/change-state/{id}`, Object.values(newState))
        return true;
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/entity-templates`;
}