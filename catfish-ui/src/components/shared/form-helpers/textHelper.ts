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

export function createGuid() {
	return Guid.create().toString() as unknown as Guid;
}
export function getTextValues(container: TextCollection | null, lang: string | null): string[] {
	if (lang)
		return container?.values?.filter(txt => txt.lang === lang).map(val => val.value) as string[]
	else
		return container?.values?.map(val => val.value) as string[]
}

export function cloneTextCollection(textCollection: TextCollection) {
	const clone = JSON.parse(JSON.stringify(textCollection)) as TextCollection;
	clone.id = createGuid();
	clone.values.forEach(txt => { txt.id = createGuid() })
	return clone;
}