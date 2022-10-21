import { createPinia } from 'pinia';

export { default as App } from './App.vue';

export const pinia = createPinia();

export { router } from '../shared/crud-object-manager/routes';
