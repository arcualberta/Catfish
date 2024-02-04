import { default as config } from "@/appsettings";
import { CrudProxy } from "./crudProxy";
//import { WebClient } from "./webClient";
export class WorkflowProxy extends CrudProxy {
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/workflow`);
    }
}
//# sourceMappingURL=workflowProxy.js.map