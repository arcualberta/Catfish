import {createRouter, createWebHashHistory, RouteRecordRaw} from 'vue-router'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'List',
        component: () => import('./views/List.vue')
    },
    {
        path: '/create',
        name: 'Create',
        component: () => import('./views/Create.vue')
    },
    {
        path: '/read/:id',
        name: 'Read',
        component: () => import('./views/Read.vue')
    },
    {
        path: '/update/:id',
        name: 'Update',
        component: () => import('./views/Update.vue')
    },
    {
        path: '/delete/:id',
        name: 'Delete',
        component: () => import('./views/Delete.vue')
    },
]

export const router = createRouter({
    history: createWebHashHistory(),
    routes, // short for `routes: routes`
})