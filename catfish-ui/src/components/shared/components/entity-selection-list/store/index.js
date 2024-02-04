import { defineStore } from "pinia";
import { default as config } from "@/appsettings";
const definedStores = new Map();
const entitySelectStoreFactory = (storeId) => {
    if (!definedStores.has(storeId)) {
        definedStores.set(storeId, defineEntitySelectStore(storeId));
    }
    return definedStores.get(storeId);
};
function defineEntitySelectStore(storeId) {
    return defineStore(`entitySelectStore/${storeId}`, {
        state: () => ({
            selectedEntityIds: [],
            entitySearchResult: null
        }),
        actions: {
            seach(entityType, searchTarget, searchText) {
                let offset = 0;
                let max = 10;
                let api = config.dataRepositoryApiRoot + "/api/entities/" + entityType + "/" + searchTarget + "/" + searchText + "/" + offset + "/" + max;
                fetch(api, {
                    method: 'GET'
                })
                    .then(response => response.json())
                    .then(data => {
                    this.entitySearchResult = data;
                })
                    .catch((error) => {
                    console.error('Load Entities API Error:', error);
                });
            }
        }
    });
}
export const useEntitySelectStore = (storeId) => {
    return entitySelectStoreFactory(storeId)();
};
/* standard pinia store
export const useEntitySelectStore = defineStore('EntitySelectStore', {
  state: () => ({
      entities: {} as EntityData[],
      entitySearchResult: null as unknown as EntitySearchResult
    }),
    
    actions:{
          seach(entityType: eEntityType, searchTarget: eSearchTarget, searchText: string){
              let offset=0;
              let max=10;
              let api = config.dataRepositoryApiRoot + "/api/entities/"+ entityType + "/" + searchTarget + "/" + searchText + "/" +offset + "/" + max ;
            fetch(api, {
                method: 'GET'
            })
            .then(response => response.json())
            .then(data => {
                    this.entitySearchResult = data as EntitySearchResult;
            })
            .catch((error) => {
                console.error('Load Entities API Error:', error);
            });
          }
      }

})
*/ 
//# sourceMappingURL=index.js.map