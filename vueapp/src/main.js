import "bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import { FontAwesomeIcon } from './plugins/font-awesome'
import "./assets/site.css";
import "bootstrap/dist/js/bootstrap.js";
import { createApp } from 'vue';
import App from './App.vue';
import router from './router';
import store from "./store";

createApp(App)
    .use(router)
    .use(store)
    .component("font-awesome-icon", FontAwesomeIcon)
    .mount("#app");