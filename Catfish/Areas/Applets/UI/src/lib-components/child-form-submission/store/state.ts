import { Guid } from 'guid-typescript'
import { FieldContainer } from "../../shared/models/fieldContainer"

//Declare State interface
export interface State {
  
    itemInstanceId: Guid | null;
    itemTemplateId: Guid | null;
    formId: Guid | null;
    form: FieldContainer | null;
    formInstances: FieldContainer[] | null;
  
}

export const state: State = {

    itemInstanceId: null,
    itemTemplateId: null,
    formId: null,
    form: null,
    formInstances: null
}
