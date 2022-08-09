import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import App from './App.vue'
import router from './router'
import Vue from 'vue'
import { BootstrapVue, IconsPlugin } from 'bootstrap-vue'

// Import Bootstrap and BootstrapVue CSS files (order is important)
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'
//import "bootstrap/dist/css/bootstrap.min.css"
//import "bootstrap"
import vue3GoogleLogin from 'vue3-google-login'

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import * as faIcons from '@fortawesome/free-solid-svg-icons'

/* add icons to the library */
library.add(faIcons.faCircleCheck)
library.add(faIcons.faCircleXmark)
library.add(faIcons.faPenToSquare)
library.add(faIcons.faCirclePlus)
library.add(faIcons.faQuestionCircle)

createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(createPinia())
    .use(router)
    // Make BootstrapVue available throughout your project
    .use(BootstrapVue)
    // Optionally install the BootstrapVue icon components plugin
    .use(IconsPlugin)
    .use(vue3GoogleLogin, {
        clientId: '589183038778-u256nlels7v2443j3h1unvtp367f80s4.apps.googleusercontent.com'
    })
    .mount('#app')
