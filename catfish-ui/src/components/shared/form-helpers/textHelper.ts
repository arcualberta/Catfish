
import { Guid } from 'guid-typescript';
import { TextCollection, Text, TextType, Field, FieldType, FieldData } from '../form-models';

/**
 * Creates a new TextColleciton object
 * @param lang
 */
export const createTextCollection = (lang: string[] | string): TextCollection => {
	const textCollection: TextCollection = {
		id: Guid.create().toString() as unknown as Guid,
		values: [] as Text[]
	} as TextCollection;

	const languages = typeof (lang) == 'string' ? [lang] : lang;

	languages.forEach(lang => {
		textCollection.values.push({
			id: Guid.create().toString() as unknown as Guid,
			lang: lang,
			value: ""
		} as Text);
	})
	return textCollection;
}

/**
 * Creates a new Text object
 * @param lang
 */
export const createText = (lang: string | null): Text => {
	const text = {
		id: Guid.create().toString() as unknown as Guid,
		value: "",
		lang: lang? lang : "en"
	} as Text

	//if (lang)
	//	text.lang = lang;

	return text;
}

export const createGuid = (): Guid => {
	return Guid.create().toString() as unknown as Guid;
}

export const getTextValue = (container: TextCollection | null, lang: string | null, separator?: string | null): string[] | string => {
	let vals: string[]
	if (lang)
		vals = container?.values?.filter(txt => txt.lang === lang).map(val => val.value) as string[]
	else
		vals = container?.values?.map(val => val.value) as string[]

	return separator ? vals.join(separator) : vals
}

export const cloneTextCollection = (textCollection: TextCollection): TextCollection => {
	const clone = JSON.parse(JSON.stringify(textCollection)) as TextCollection;
	clone.id = createGuid();
	clone.values.forEach(txt => { txt.id = createGuid() })
	return clone;
}

export const getConcatenatedValues = (container: FieldData,  separator: string ): string => {
	var vals: string="";
	var texts: string[]=[];
	
	if(container.multilingualTextValues && container.multilingualTextValues?.length > 0)
	{
		container.multilingualTextValues.forEach((multiTextVal)=>{
			vals += getTextValue(multiTextVal as TextCollection,null,  separator) as string;
		})
	}else if(container.monolingualTextValues && container.monolingualTextValues?.length > 0){
       
		container.monolingualTextValues.forEach((text)=>{
			texts.push(text.value);
		});
		vals = texts?.join(separator);
	}
	
	return vals;
}
