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
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
