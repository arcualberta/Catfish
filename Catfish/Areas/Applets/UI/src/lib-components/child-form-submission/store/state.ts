import { Guid } from 'guid-typescript'
import { FieldContainer, Option } from '../../shared/models/fieldContainer'
import { Text } from '../../shared/models/textModels'

//Declare State interface
export interface State {
  
    itemInstanceId: Guid | null;
    itemTemplateId: Guid | null;
    formId: Guid | null;
    form: FieldContainer | null;
    flattenedTextModels: { [key: string]: Text };
    flattenedOptionModels: { [key: string]: Option };
    formInstances: FieldContainer[];
  
}

export const state: State = {

    itemInstanceId: null,
    itemTemplateId: null,
    formId: null,
    form: null,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    formInstances: []
}
