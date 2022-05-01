import { Guid } from "guid-typescript";

export interface Page {
	id: Guid;
	typeId: string;
	title: string;
	created: Date;
	lastModified: Date;
	permissions: string[];
	isPublished: boolean;
	published: Date;
	permalink: string;
	slug: string;
	isCommentsOpen: boolean;
	commentCount: number;
	blocks: Block[];
	isHidden: boolean;
	excerpt: string;
	primaryImage: ImageField;
	sortOrder: number;
	parentId: Guid | null;
	siteId: Guid;
	isStartPage: boolean;
}


export interface Block {
	id: Guid;
	type: string;
}

export interface Grid extends Block {
	items: Card[];
}

export interface Card extends Block {
	cardImage: ImageField;
	cardTitle: TextField;
	cardSubTitle: TextField;
	hasAModal: CheckBoxField;
	modalSize: SelectField;
	isModalCenteredOnTheScreen: CheckBoxField;
	modalImage: ImageField;
	imagePosition: SelectField;
	modalTitle: TextField;
	modalSubTitle: TextField;
	modalDescription: TextField;
	emailAddress: TextField;
	buttonLink: TextField;
	buttonText: TextField;
	buttonColor: TextField;
	preventUserFromExitingOnOutsideClick: CheckBoxField;
}


export interface TextField {
	value: string;
}

export interface CheckBoxField {
	value: boolean;
}

export interface SelectField {
	value: number | string;
}

export interface ImageField {
	id: Guid;
	media: Media;
	hasValue: boolean;
}

export interface Media {
	id: Guid;
	type: number;
	filename: string;
	contentType: string;
	title: string;
	altText: string;
	description: string;
	size: number;
	publicUrl: string;
	width: number;
	height: number;
	created: Date;
	lastModified: Date;
}

export interface DataSource {
	pageId: Guid;
	blockId: Guid | null;
}

