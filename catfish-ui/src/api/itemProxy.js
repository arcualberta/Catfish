import { default as config } from "@/appsettings";
//import { EntityData, TemplateEntry } from "@/components/entity-editor/models";
//import { EntityTemplate } from "@/components/entity-template-builder/models";
//import { EntityEntry } from "@/components/shared/models/listEntries";
//import { Guid } from "guid-typescript";
import { CrudProxy } from "./crudProxy";
//import { WebClient } from "./webClient";
export class ItemProxy extends CrudProxy {
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/items`);
    }
}
//# sourceMappingURL=itemProxy.js.map