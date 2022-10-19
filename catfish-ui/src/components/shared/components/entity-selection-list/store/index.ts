import { EntityData, EntityEntry, EntitySearchResult } from "@/components/entity-editor/models";
import { eEntityType, eSearchTarget } from "@/components/shared/constants";
import { defineStore } from "pinia";
import { default as config } from "@/appsettings";
import { Guid } from "guid-typescript";


const definedStores = new Map<string, ReturnType<typeof defineEntitySelectStore>>();
const entitySelectStoreFactory = (storeId: string) => {
  if (!definedStores.has(storeId)) {
    definedStores.set(storeId, defineEntitySelectStore(storeId));
  }

  return definedStores.get(storeId) as ReturnType<
    typeof defineEntitySelectStore
  >;
};

function defineEntitySelectStore<Id extends string>(storeId: Id) {
  return defineStore(`entitySelectStore/${storeId}`, {
    state: () => ({
        selectedEntityIds: [] as Guid[],
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
  });
}
export const useEntitySelectStore = (storeId: string) => {
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