import {  Option } from '../models/fieldContainer'
import { Text } from '../models/textModels';

export interface FlattenedFormFiledState {
	flattenedTextModels: { [key: string]: Text };
	flattenedOptionModels: { [key: string]: Option };
	flattenedFileModels: { [key: string]: File[] };
}

