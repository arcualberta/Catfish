import { default as config } from "@/appsettings";

import {api} from '@arc/arc-foundation'

export class FormProxy extends api.CrudProxy {   
    constructor(jwtToken?: string) {
        super(`${config.dataRepositoryApiRoot}/api/forms`, jwtToken)
    }
}
