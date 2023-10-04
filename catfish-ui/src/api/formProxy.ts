import { default as config } from "@/appsettings";

import {CrudProxy} from '@arc/arc-foundation'

export class FormProxy extends CrudProxy {   
    constructor(jwtToken?: string) {
        super(`${config.dataRepositoryApiRoot}/api/forms`, jwtToken)
    }
}
