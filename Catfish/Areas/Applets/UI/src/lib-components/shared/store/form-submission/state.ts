import { Option } from '../../models/fieldContainer'
import { Text } from '../../models/textModels'

//Declare State interface
export interface FlattenedFormFiledState {
	flattenedTextModels: { [key: string]: Text };
	flattenedOptionModels: { [key: string]: Option };
}
