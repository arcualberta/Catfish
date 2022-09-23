import { Guid } from "guid-typescript";

export interface SelectableOption
{
    value: Guid,
    text: string
};

export interface OptionGroup {
    label: string;
    options: OptionEntry[];
}

export type OptionEntry = SelectableOption | OptionGroup;
