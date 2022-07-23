import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/form-builder/:id?',
        name: 'formBuilder',
        component: () => import('./views/FormBuilder.vue')
    },
    {
        path: '/form-submission/:formId?',
        name: 'newFormSubmission',
        component: () => import('./views/FormSubmission.vue')
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
