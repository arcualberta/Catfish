import { Guid } from "guid-typescript";

export interface GoogleIdentityResult {
    clientId: string;
    credential: string;
}

export interface LoginResult {
    id: Guid | null;
    name: string | null;
    emai: string | null;
    globalRoles: string[] | null;
    success: boolean | null;
}