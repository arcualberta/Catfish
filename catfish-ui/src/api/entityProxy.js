import { default as config } from "@/appsettings";
import { CrudProxy } from "./crudProxy";
export class EntityProxy extends CrudProxy {
    //let apiRoot = `${config.dataRepositoryApiRoot}/api/entities`;
    // /*`${config.dataRepositoryApiRoot}/api/entities`*/
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/entities`);
    }
}
//# sourceMappingURL=entityProxy.js.map