import Vue from 'vue'
import Vuex from 'vuex'
import VueRouter from 'vue-router'
import App from './App.vue'
import VueResource from 'vue-resource';

import Login from './components/Login'
import OffersView from './components/OffersView'

Vue.use(Vuex);
Vue.use(VueResource);
Vue.use(VueRouter);

const store = new Vuex.Store({
  state: {
    user: "",
    authToken: "",
  },
  mutations: {
    clearCredentials(state, user, token) {
      state.user = null;
      state.authToken = "";
    },
    setCredentials(state, user, token){
      state.user = user;
      state.authToken = token;
    },
  },
  getters: {
    getUser(state){
      return state.user;
    },
    getAuthToken(state){
      return state.authToken;
    },
  }
});

const routes = [
  {
    path: "/",
    name: "login",
    component: Login
  },
  {
    path: "/offers",
    name: "offers",
    component: OffersView
  }
]

const router = new VueRouter({routes});

new Vue({
  el: '#app',
  render: h => h(App),
  store,
  router
})