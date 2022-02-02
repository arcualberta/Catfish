import { Guid } from "guid-typescript";
import { FieldContainer } from "../../shared/models/fieldContainer";
import { TextCollection } from "../../shared/models/textModels"

export interface EntityModel {
	id: Guid;
	status: string;
	modelType: string;
	metadataSets: {
		$type: string;
		$values: FieldContainer[];
	};
	dataContainer: {
		$type: string;
		$values: FieldContainer[];
	};
	name: TextCollection | null;
	description: TextCollection | null;
	statusId: Guid | null;
}