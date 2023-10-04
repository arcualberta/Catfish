import { default as config } from "@/appsettings";
import { CrudProxy } from "./crudProxy";
import { WebClient } from "./webClient";


export class EntityProxy extends CrudProxy{
    //let apiRoot = `${config.dataRepositoryApiRoot}/api/entities`;
    // /*`${config.dataRepositoryApiRoot}/api/entities`*/
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/entities`);
    } 

   /* static async loadEntities(entityType: eEntityType, searchTarget: eSearchTarget, searchText: string, offset: number, max?: number){
        let api = `${config.dataRepositoryApiRoot}/api/entities/{entityType}/{searchTarget}/{searchText}/{offset}/{max}` ;

        var response = await WebClient.get(`${api}`)
        return response;
    }*/
}