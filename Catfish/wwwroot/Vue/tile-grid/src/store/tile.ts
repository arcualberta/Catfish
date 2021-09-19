import { Guid } from "guid-typescript";

//Declare Tile interface which has the same structure as its
//c# counterpart in the server-side code
export interface Tile {
  id: Guid,
  title: string,
  content: string,
  thumbnail: URL,
  created: Date,
  objectUrl: URL
}
