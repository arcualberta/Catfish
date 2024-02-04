import { default as config } from "@/appsettings";
import { CrudProxy } from "./crudProxy";
export class FormDataProxy extends CrudProxy {
    constructor() {
        super(`${config.dataRepositoryApiRoot}/api/form-submissions`);
    }
}
//# sourceMappingURL=formDataProxy.js.map