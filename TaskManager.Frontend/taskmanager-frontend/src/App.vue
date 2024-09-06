<template>
  <div id="app">
    <!-- Aqui será injetado o componente correspondente à rota -->
    <router-view />
  </div>
</template>

<script>
export default {
  name: 'App',
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  background-color: #e2e2e5;
  width: 100%;
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  font-size: small;
}
</style>
