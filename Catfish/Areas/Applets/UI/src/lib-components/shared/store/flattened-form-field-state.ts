import { Option } from '../models/fieldContainer'
import { Text } from '../models/textModels';

export interface FlattenedFormFiledState {
	modified: boolean;
	flattenedTextModels: { [key: string]: Text };
	flattenedOptionModels: { [key: string]: Option };
	flattenedFileModels: { [key: string]: File[] };
}

export const flattenedFormFiledState: FlattenedFormFiledState = {
    modified: false,
    flattenedTextModels: {},
    flattenedOptionModels: {},
    flattenedFileModels: {},
}

