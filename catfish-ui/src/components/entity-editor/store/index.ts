import { Guid } from 'guid-typescript';
import { defineStore } from 'pinia';
import { Entity,  TemplateEntry } from '../models';
import {EntityTemplate} from '../../entity-template-builder/models'


export const useEntityEditorStore = defineStore('EntityEditorStore', {
    state: () => ({
        id: null as Guid | null,
        templates: [] as TemplateEntry[],
        entityTemplate: null as EntityTemplate | null,
        entity: null as Entity | null

    }),
    actions: {
       
    }
});