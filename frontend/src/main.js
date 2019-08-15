
/* UGLY POLYFILL FOR IE11 CUSTOM EVENTS CAUSE VUE DOES NOT SUPPORT EVENTS FROM DIRECTIVES WELL */
(function () {
  if ( typeof window.CustomEvent === "function" ) return false; //If not IE

  function CustomEvent ( event, params ) {
    params = params || { bubbles: false, cancelable: false, detail: undefined };
    var evt = document.createEvent( 'CustomEvent' );
    evt.initCustomEvent( event, params.bubbles, params.cancelable, params.detail );
    return evt;
   }

  CustomEvent.prototype = window.Event.prototype;

  window.CustomEvent = CustomEvent;
})();
/* UGLY POLYFILL FOR IE11 CUSTOM EVENTS CAUSE VUE DOES NOT SUPPORT EVENTS FROM DIRECTIVES WELL */


import Vue from "vue";
import App from "./App.vue";
import Router from "vue-router";
import Home from "./views/Home.vue";
import Intro from "./views/Intro.vue";
import Countdown from "./views/Countdown.vue";
import VueBrowserUpdate from "vue-browserupdate";
import cookieconsent from 'vue-cookieconsent-component'

Vue.component('cookie-consent', cookieconsent)

// Import thema
import "@/themes/hacker/theme.scss";
import { Title, IntroText } from "@/themes/hacker/theme.js";

// import "@/themes/basic/theme.scss";
// import { Title, IntroText } from "@/themes/basic/theme.js";

Vue.config.productionTip = false;

Vue.directive("introtext", IntroText);
Vue.directive("title", Title);

Vue.use(VueBrowserUpdate, {
  options: {
    required: { i: 8, f: 25, o: 17, s: 9, c: 22 },
    reminder: 24
  },
  test: false,
  l: false
});

Vue.use(Router);

const router = new Router({
  mode: "abstract",
  routes: [
    {
      path: "/",
      name: "home-page",
      component: Home
    },
    {
      path: "/intro",
      name: "intro-page",
      component: Intro
    },
    {
      path: "/countdown",
      name: "countdown-page",
      component: Countdown
    }
  ]
});

router.replace("/");

new Vue({
  router,
  render: h => h(App)
}).$mount("#app");
