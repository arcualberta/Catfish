import { Guid } from "guid-typescript";

export interface DataAttribute {
  name: string | null;
  value: string | null;
}

export interface ConfigParams {
  pageId: Guid | null;
  blockId: Guid | null;
  params: Array<Param>;
}

export interface Param {
  key: string | null;
  val: object | null;
}
