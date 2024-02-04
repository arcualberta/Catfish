import { createRouter, createWebHistory } from 'vue-router';
import Home from './views/Home.vue';
const routes = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/login',
        name: 'login',
        component: () => import('./views/Login.vue')
    },
    {
        path: '/form-builder/:id?',
        name: 'formBuilder',
        component: () => import('./views/FormBuilder.vue')
    },
    {
        path: '/workflow-builder/:id?',
        name: 'workflow',
        component: () => import('./views/WorkflowBuilder.vue')
    },
    {
        path: '/new-form-submission/:formId?',
        name: 'newFormSubmission',
        component: () => import('./views/NewFormSubmission.vue')
    },
    {
        path: '/edit-form-submission/:submissionId?',
        name: 'editFormSubmission',
        component: () => import('./views/EditFormSubmission.vue')
    },
    {
        path: '/sandbox',
        name: 'sandbox',
        component: () => import('./views/Sandbox.vue')
    },
    {
        path: '/google-calendar',
        name: 'googleCaendar',
        component: () => import('./views/GoogleCalendar.vue')
    },
    {
        path: '/entity-template-builder',
        name: 'entityTemplateBuilder',
        component: () => import('./views/EntityTemplateBuilder.vue')
    },
    {
        path: '/edit-entity-template/:id',
        name: 'editEntityTemplate',
        component: () => import('./views/EditEntityTemplate.vue')
    },
    {
        path: '/entity-editor/',
        name: 'entityEditor',
        component: () => import('./views/EntityEditor.vue')
    },
    {
        path: '/edit-entity-editor/:id',
        name: 'editEntityEditor',
        component: () => import('./views/EntityEditor.vue')
    },
    {
        path: '/solr-search-panel/',
        name: 'solrSearchPanel',
        component: () => import('./views/SolrSearchPanel.vue')
    },
    {
        path: '/api-test/',
        name: 'ApiTest',
        component: () => import('./views/ApiTest.vue')
    },
    {
        path: '/test-page/',
        name: 'TestPage',
        component: () => import('./views/TestPage.vue')
    },
];
const router = createRouter({
    history: createWebHistory(),
    routes
});
export default router;
//# sourceMappingURL=router.js.map