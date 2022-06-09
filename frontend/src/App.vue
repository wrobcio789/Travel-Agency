<template>
  <div id="app">
    '<snackbar baseSize="1rem" ref="snackbar" :holdTime="5000" position="top-center"/>'
    <app-header></app-header>
    <router-view/>
    <app-footer></app-footer>
  </div>
</template>

<script>

import AppHeader from './components/AppHeader.vue';
import AppFooter from './components/AppFooter.vue';

export default {
  components: { AppHeader, AppFooter },
  name: 'app'
}
</script>

<style>
body {
  margin: 0;
}
</style>
