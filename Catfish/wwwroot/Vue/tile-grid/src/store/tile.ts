import { Guid } from "guid-typescript";

//Declare Tile
export type Tile = {
  id: Guid,
  title: string,
  content: string,
  thumbnail: URL,
  created: Date,
  objectUrl: URL
}
