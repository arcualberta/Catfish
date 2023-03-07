import { default as config } from "@/appsettings";
import { TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";
import { ListEntry } from "@/components/shared";
import { Guid } from "guid-typescript";
import { WebClient } from "./webClient";

export class CrudProxy{

    private _apiRoot: string;

    constructor(apiRoot: string) {
        this._apiRoot = apiRoot;
    }

    async List<T>(): Promise<T[]> {
        var response = await WebClient.get(this._apiRoot)
        var data = await response.json()
        return data as T[]   
    }

    async Get<T>(id: Guid): Promise<T> {
        var response = await WebClient.get(`${this._apiRoot}/${id}`)
        var data = await response.json()
        return data as T   
    }

    async Post(entityTemplate: EntityTemplate): Promise<boolean> {
        let newIdCreated = false
        try{
            if(!entityTemplate.id || entityTemplate.id == Guid.parse(Guid.EMPTY)) {
                entityTemplate.id = Guid.create().toString() as unknown as Guid
                newIdCreated = true
            }
            var response = await WebClient.postJson(`${this._apiRoot}`, entityTemplate)
            return true;
        }
        catch(e){
            if(newIdCreated){
                entityTemplate.id = Guid.parse(Guid.EMPTY)
            }
            throw e;
        }
    }    

    async Put(id: Guid, data: object): Promise<boolean> {
        var response = await WebClient.putJson(`${this._apiRoot}/${id}`, data)
        return true
    }

    async Delete(id: Guid): Promise<boolean> {
        var response = await WebClient.delete(`${this._apiRoot}/${id}`)
        return response.ok
    }
}