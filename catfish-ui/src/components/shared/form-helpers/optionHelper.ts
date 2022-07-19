import { Guid } from 'guid-typescript';
import { Option, ExtensionType, TextCollection, Text } from '../form-models';
import { createTextCollection } from '../../shared/form-helpers'

export function createOption(languages: string[]): Option {
	return {
		id: Guid.create().toString() as unknown as Guid,
		isExtendedInput: ExtensionType.None,
		optionText: createTextCollection(languages)
	} as Option;
}