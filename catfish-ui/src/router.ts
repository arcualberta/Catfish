import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import Home from './views/Home.vue'
import FormBuilder from './views/FormBuilder.vue'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'home',
        component: Home
    },
    {
        path: '/form-builder',
        name: 'formBuilder',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: FormBuilder // () => import('./views/FormBuilder.vue')
    },
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router
