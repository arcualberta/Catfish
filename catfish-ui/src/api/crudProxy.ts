import { Guid } from "guid-typescript";
import { WebClient } from "./webClient";

export interface ObjectId{
    id: Guid | null
 }
export class CrudProxy {
 
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

    async Post<T extends ObjectId>(data: T): Promise<boolean> {
        let newIdCreated = false
        try{
            if(!data.id || data.id == Guid.parse(Guid.EMPTY)) {
                data.id = Guid.create().toString() as unknown as Guid
                newIdCreated = true
            }
            var response = await WebClient.postJson(`${this._apiRoot}`, data)
            
            return true;
        }
        catch(e){
            if(newIdCreated){
                data.id = Guid.parse(Guid.EMPTY)
            }
            console.error(e)
            return false;
        }
    }    

    async Put<T extends ObjectId>(data: T): Promise<boolean> {
        var response = await WebClient.putJson(`${this._apiRoot}/${data.id}`, data)
        return true
    }

    async Delete(id: Guid): Promise<boolean> {
        var response = await WebClient.delete(`${this._apiRoot}/${id}`)
        return response.ok
    }
}