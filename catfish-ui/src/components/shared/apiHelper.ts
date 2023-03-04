import { useLoginStore } from "../login/store"


export class ApiHelper{

    static get = (url:string): Promise<Response> => this.invoke(url, "GET")

    static postJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "POST", payload)
    
    static putJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "PUT", payload)

    static patchJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "PATCH", payload)

    static delete = (url:string): Promise<Response> => this.invoke(url, "DELETE")

    //#region Private Methods
    private static getJwtToken = (): string =>  useLoginStore().jwtToken;
    
    private static invoke(url:string, method:string): Promise<Response> {
        var promise = fetch(url, {
            method: method,
            headers: {
                'Authorization': `bearer ${this.getJwtToken()}`
            }
        })
        return promise
    }

    private static sendJson(url: string, method: string, payload: object): Promise<Response> {
        var promise = fetch(url,
            {
                body: JSON.stringify(payload),
                method: method,
                headers: {
                    'encType': 'multipart/form-data',
                    'Content-Type': 'application/json',
                    'Authorization': `bearer ${this.getJwtToken()}`
                },
            })
        return promise
    }
    //#endregion
}