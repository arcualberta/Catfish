import { EntityData, EntitySearchResult } from "@/components/entity-editor/models";
import { eEntityType, eSearchTarget } from "@/components/shared/constants";
import { defineStore } from "pinia";
import { default as config } from "@/appsettings";

/*export const useEntitySelectStore = (storeId: string) => defineStore(`entitySelect/${storeId}`, {
    state: () => ({
      entities: {} as EntityData[],
      entitySearchResult: null as unknown as EntitySearchResult
    }),
    
    actions:{
          seach(entityType: eEntityType, searchTarget: eSearchTarget, searchText: string){
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
  })(); */

  export const useEntitySelectStore = defineStore('EntitySelectStore', {
    state: () => ({
        entities: {} as EntityData[],
        entitySearchResult: null as unknown as EntitySearchResult
      }),
      
      actions:{
            seach(entityType: eEntityType, searchTarget: eSearchTarget, searchText: string){
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
