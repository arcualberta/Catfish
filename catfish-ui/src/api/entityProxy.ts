import { default as config } from "@/appsettings";
import { CrudProxy } from "./crudProxy";


export class EntityProxy extends CrudProxy{
    //const api = `${config.dataRepositoryApiRoot}/api/entities/${entityId}`;
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/entities`)
    } 
}