import { createApp } from "vue";
import App from "./App.vue";
import { store, key } from "./store";
import axios from "axios";
import VueAxios from "vue-axios";
const app = createApp(App);
app.use(VueAxios, axios);
app.provide("axios", app.config.globalProperties.axios); // provide 'axios'
app.use(store, key).mount("#app");
//# sourceMappingURL=main.js.map