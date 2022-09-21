import { Guid } from 'guid-typescript';
import { Option, TextCollection, Text } from '../form-models';
import { createTextCollection } from '../../shared/form-helpers'

export function createOption(languages: string[], optionText: TextCollection | null): Option {
	const opt = {
		id: Guid.create().toString() as unknown as Guid,
		isExtendedInput: false,
		isExtendedInputRequired: false,
		optionText: optionText ? optionText : createTextCollection(languages)
	} as Option;
	return opt;
}
export const getOptionText = (option: Option, lang: string | null): string[] | string  => getOptionValues(option.optionText, lang)

export function getOptionValues(optionText: TextCollection | null, lang: string | null): string[] | string {
	
	if (lang)
		return optionText?.values?.filter(txt => txt.lang === lang).map(val => val.value)?.at(0) as string
	else
		return optionText?.values?.map(val => val.value) as string[]
}

export const cloneOption = (option: Option) => {
	const clone = JSON.parse(JSON.stringify(option)) as Option;
	clone.id = Guid.create();
	clone.optionText.id = Guid.create();
	clone.optionText.values.forEach(txt => txt.id = Guid.create())
	return clone;
}