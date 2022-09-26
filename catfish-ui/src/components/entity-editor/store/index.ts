import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { Entity,  TemplateEntry } from '../models';
import {EntityTemplate} from '../../entity-template-builder/models'
import { default as config } from "@/appsettings";


export const useEntityEditorStore = defineStore('EntityEditorStore', {
    state: () => ({
        id: null as Guid | null,
        templates: [] as TemplateEntry[],
        entityTemplate: null as EntityTemplate | null,
        entity: null as Entity | null

    }),
    actions: {
    loadTemplates(){
        const api = `${config.dataRepositoryApiRoot}/api/entity-templates/`;
            
            fetch(api, {
                method: 'GET'
            })
                .then(response => response.json())
                .then(data => {
                    this.templates = data as  TemplateEntry[];
                })
                .catch((error) => {
                    console.error('Load Templates API Error:', error);
                });
       }
    }
});