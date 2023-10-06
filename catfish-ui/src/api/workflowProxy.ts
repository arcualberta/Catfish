import { default as config } from "@/appsettings";
import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
import { EntityTemplate } from "@/components/entity-template-builder/models";

import { EntityEntry } from "@/components/shared/models/listEntries";
import { Workflow } from "@/components/workflow-builder/models";
import { Guid } from "guid-typescript";
import { CrudProxy, ObjectId } from "./crudProxy";
//import { WebClient } from "./webClient";

export class WorkflowProxy extends CrudProxy{

    
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/workflow`)
    }  
 /*
    private static _crudProxy: CrudProxy = new CrudProxy(`${config.dataRepositoryApiRoot}/api/workflow`);
    
    static async List (): Promise<Workflow[]> {
        return await this._crudProxy.List<Workflow>();
    }

    static async Get(id: Guid): Promise<Workflow> {
        return await this._crudProxy.Get<Workflow>(id);
    }

    static async Post<Workflow extends ObjectId>(workflow: Workflow): Promise<boolean> {
       return await this._crudProxy.Post<Workflow>(workflow);
       
    }    

    static async Put(workflow: Workflow): Promise<boolean> {
        return await this._crudProxy.Put(workflow.id as Guid, workflow); 
    } 
    
    static async Delete(id: Guid): Promise<boolean>{
        return await this._crudProxy.Delete(id)
    }

    private static getApiRoot = () => `${config.dataRepositoryApiRoot}/api/workflow`;
    */
}