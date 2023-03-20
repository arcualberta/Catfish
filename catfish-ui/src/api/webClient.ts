import { useLoginStore } from "../components/login/store"
import { useEntityEditorStore } from "@/components"
import { Guid } from "guid-typescript"


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

    //Post/put for Entity
   /* private static async postEntity=(url: string,method: string, payload: object, files:File[]|null, fileKeys: string[]|null):Promise<Response>{
       
        const entityEditorStore = useEntityEditorStore();           
      
            var formData = new FormData();
             //update fileReference
            let fileKeyIdx=0;
            files?.forEach(file => {
                let newFileKey = entityEditorStore.addFileReference(file, Guid.parse(fileKeys![fileKeyIdx]));
                
                fileKeyIdx++;
            });
            

            formData.append('value', JSON.stringify(payload));

            files?.forEach(file => {
                formData.append('files', file);
            });
            
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
    }*/
    //#endregion
}