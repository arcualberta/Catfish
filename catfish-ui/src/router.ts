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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
