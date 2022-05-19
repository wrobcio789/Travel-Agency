import Vue from 'vue'
import Vuex from 'vuex'
import VueRouter from 'vue-router'
import App from './App.vue'
import VueResource from 'vue-resource';

import Login from './components/Login'
import OffersView from './components/OffersView'
import TripView from './components/TripView'
import vSelect from 'vue-select'

import 'vue-select/dist/vue-select.css';
import VueNumericInput from 'vue-numeric-input';
import Datepicker from 'vuejs-datepicker';

Vue.use(Vuex);
Vue.use(VueResource);
Vue.use(VueRouter);

Vue.component('v-select', vSelect)
Vue.component('datepicker', Datepicker)

Vue.use(VueNumericInput)

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
  },
  {
    path: "/trip",
    name: "trip",
    props: true,
    component: TripView,
  }
]

const router = new VueRouter({routes});

new Vue({
  el: '#app',
  render: h => h(App),
  store,

  router
})