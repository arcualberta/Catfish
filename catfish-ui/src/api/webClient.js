import { useLoginStore } from "../components/login/store";
export class WebClient {
    static get = (url) => this.invoke(url, "GET");
    static postJson = (url, payload) => this.sendJson(url, "POST", payload);
    static putJson = (url, payload) => this.sendJson(url, "PUT", payload);
    static patchJson = (url, payload) => this.sendJson(url, "PATCH", payload);
    static delete = (url) => this.invoke(url, "DELETE");
    //#region Private Methods
    static getJwtToken = () => useLoginStore().jwtToken;
    static async invoke(url, method) {
        var response = await fetch(url, {
            method: method,
            headers: {
                'Authorization': `bearer ${this.getJwtToken()}`
            }
        });
        if (!response.ok) {
            throw new Error(`Code ${response.status} returned by ${url}`);
        }
        return response;
    }
    static async sendJson(url, method, payload) {
        var response = await fetch(url, {
            body: JSON.stringify(payload),
            method: method,
            headers: {
                'encType': 'multipart/form-data',
                'Content-Type': 'application/json',
                'Authorization': `bearer ${this.getJwtToken()}`
            },
        });
        if (!response.ok) {
            throw new Error(`Code ${response.status} returned by ${url}`);
        }
        return response;
    }
}
//# sourceMappingURL=webClient.js.map