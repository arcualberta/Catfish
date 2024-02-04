import { Guid } from "guid-typescript";
import { WebClient } from "./webClient";
export class CrudProxy {
    _apiRoot;
    constructor(apiRoot) {
        this._apiRoot = apiRoot;
    }
    async List() {
        var response = await WebClient.get(this._apiRoot);
        var data = await response.json();
        return data;
    }
    async Get(id) {
        var response = await WebClient.get(`${this._apiRoot}/${id}`);
        var data = await response.json();
        return data;
    }
    async Post(data) {
        let newIdCreated = false;
        try {
            if (!data.id || data.id == Guid.parse(Guid.EMPTY)) {
                data.id = Guid.create().toString();
                newIdCreated = true;
            }
            var response = await WebClient.postJson(`${this._apiRoot}`, data);
            return true;
        }
        catch (e) {
            if (newIdCreated) {
                data.id = Guid.parse(Guid.EMPTY);
            }
            console.error(e);
            return false;
        }
    }
    async Put(data) {
        var response = await WebClient.putJson(`${this._apiRoot}/${data.id}`, data);
        return true;
    }
    async Delete(id) {
        var response = await WebClient.delete(`${this._apiRoot}/${id}`);
        return response.ok;
    }
}
//# sourceMappingURL=crudProxy.js.map