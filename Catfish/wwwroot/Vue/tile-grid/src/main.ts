import { createApp } from 'vue'
import App from './App.vue'
import store from './store'
import './assets/global.css'

const vue3App = createApp(App).use(store).mount('#app')
