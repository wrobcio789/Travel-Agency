<template>
  <div id="app">
    <app-header></app-header>
    <router-view class="content"/>
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
html, body {
  margin: 0;
  height: 100%;
}

#app {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.content {
  flex: 5;
  overflow: auto;
}
</style>
