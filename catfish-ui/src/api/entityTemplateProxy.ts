import { default as config } from "@/appsettings";
import { TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { ReturnVoid } from "@/components/form-submission/__VLS_types";
import { Guid } from "guid-typescript";
import { WebClient } from "./webClient";

export class EntityTemplateProxy{

    static async List(): Promise<TemplateEntry[]> {
        var response = await WebClient.get(this.getApiRoot())
        var data = await response.json()
        return data as TemplateEntry[]   
    }

    static async Get(id: Guid): Promise<EntityTemplate> {
        var response = await WebClient.get(`${this.getApiRoot()}/${id}`)
        var data = await response.json()
        return data as EntityTemplate   
    }

    static async Post(entityTemplate: EntityTemplate): Promise<boolean> {
        let newIdCreated = false
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
        }
    }    

    static async Put(entityTemplate: EntityTemplate): Promise<boolean> {
        var response = await WebClient.putJson(`${this.getApiRoot()}`, entityTemplate)
        return true
    }    

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/entity-templates`;
}