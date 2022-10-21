import {createRouter, createWebHashHistory, RouteRecordRaw} from 'vue-router'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'list',
        component: () => import('./views/List.vue')
    },
    {
        path: '/create',
        name: 'Create',
        component: () => import('./views/Edit.vue')
    },
    {
        path: '/details/:id',
        name: 'Details',
        component: () => import('./views/Details.vue')
    },
    {
        path: '/edit/:id',
        name: 'Edit',
        component: () => import('./views/Edit.vue')
    },
]

export const router = createRouter({
    history: createWebHashHistory(),
    routes, // short for `routes: routes`
})