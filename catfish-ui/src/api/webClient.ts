import { useLoginStore } from "../components/login/store"


export class WebClient{

    static get = (url:string): Promise<Response> => this.invoke(url, "GET")

    static postJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "POST", payload)
    
    static putJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "PUT", payload)

    static patchJson = (url:string, payload: object): Promise<Response>  => this.sendJson(url, "PATCH", payload)

    static delete = (url:string): Promise<Response> => this.invoke(url, "DELETE")

    //#region Private Methods
    private static getJwtToken = (): string =>  useLoginStore().jwtToken;
    
    private static async invoke(url:string, method:string): Promise<Response> {
       
        var response = await fetch(url, {
            method: method,
            headers: {
                'Authorization': `bearer ${this.getJwtToken()}`
            }
        })

        if(!response.ok){
            throw new Error(`Code ${response.status} returned by ${url}`)
        }

        return response
    }

    private static async sendJson(url: string, method: string, payload: object): Promise<Response> {
        var response = await fetch(url, {
            body: JSON.stringify(payload),
            method: method,
            headers: {
                'encType': 'multipart/form-data',
                'Content-Type': 'application/json',
                'Authorization': `bearer ${this.getJwtToken()}`
            },
        })

        if(!response.ok){
            throw new Error(`Code ${response.status} returned by ${url}`)
        }
 
        return response
    }
    //#endregion
}