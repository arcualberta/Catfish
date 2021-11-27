import { Guid } from "guid-typescript";
import { KeywordQueryModel } from "./keywords";

export interface ResultItem {
  id: Guid;
  title: string;
  subtitle: string;
  categories: string[];
  content: string;
  thumbnail: URL;
  date: Date;
  detailedViewUrl: URL;
}

export interface SearchOutput {
  items: ResultItem[];
  first: number;
  last: number;
  count: number;
}

export interface SearchParams {
  pageId: Guid;
  blockId: Guid;
  keywords: KeywordQueryModel;
  offset: number;
  max: number;
}