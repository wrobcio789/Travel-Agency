<template>
  <div id="app">
    <app-header></app-header>
    <router-view/>
  </div>
</template>

<script>

import AppHeader from './components/AppHeader.vue';

export default {
  components: { AppHeader },
  name: 'app'
}
</script>

<style>
body {
  margin: 0;
}
</style>
