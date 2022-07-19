import { Guid } from 'guid-typescript';
import { TextCollection, Text } from '../form-models';

export function createTextCollection(languages: string[]): TextCollection {
	const textCollection: TextCollection = {
		id: Guid.create().toString() as unknown as Guid,
		values: [] as Text[]
	} as TextCollection;

	languages.forEach(lang => {
		textCollection.values.push({
			id: Guid.create().toString() as unknown as Guid,
			lang: lang
		} as Text)
	})
	return textCollection;
}

