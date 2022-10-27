import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'

const routes: Array<RouteRecordRaw> = [
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
        path: '/entity-template-builder/:templateId',
        name: 'updateEntityTemplate',
        component: () => import('./views/EntityTemplateBuilder.vue')
    },
    
    {
        path: '/edit-entity-template/:templateId',
        name: 'editEntityTemplate',
        component: () => import('./views/EditEntityTemplate.vue')
    },
    {
        path: '/entity-editor/',
        name: 'entityEditor',
        component: () => import('./views/EntityEditor.vue')
    },
    {
        path: '/edit-entity-editor/:entityId',
        name: 'editEntityEditor',
        component: () => import('./views/EntityEditor.vue')
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
