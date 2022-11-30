import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import App from './App.vue'
import router from './router'

/* Import Bootstrap and BootstrapVue CSS files (order is important) */
import BootstrapVue3 from 'bootstrap-vue-3'
import IconsPlugin from 'bootstrap-vue-3'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue-3/dist/bootstrap-vue-3.css'

/* Import Google Login */
import vue3GoogleLogin from 'vue3-google-login'

/**
 *  Importing font awesome icon component with the library of icons selected for this application.
 *  To update which icons should be included, please add them to the library in the library-exports.ts file.
 *  */
import { FontAwesomeIcon } from './library-exports'


createApp(App)
    .component('font-awesome-icon', FontAwesomeIcon)
    .use(createPinia())
    .use(router)
    .use(BootstrapVue3) // Make BootstrapVue available throughout your project
    .use(IconsPlugin) // Optionally install the BootstrapVue icon components plugin
    .use(vue3GoogleLogin, {
        clientId: '589183038778-u256nlels7v2443j3h1unvtp367f80s4.apps.googleusercontent.com'
    })
    .mount('#app')
