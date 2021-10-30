import { Guid } from "guid-typescript";

/*
 *  
 */
 export interface Item {
  id: Guid,
  title: string,
  subtitle: string,
  categories: string[]
  content: string,
  thumbnail: URL,
  date: Date,
  detailedViewUrl: URL
}

