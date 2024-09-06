<template>
  <div id="app">
    <div id="login">
      <div id="description">
        <h1>Task Manager</h1>
      </div>
      <div id="form">
        <form @submit.prevent="doLogin">
          <label for="username">Username</label>
          <input type="text" id="username" v-model="username" placeholder="" autocomplete="off">

          <label for="password">Password</label>
          <i class="fas" :class="[passwordFieldIcon]" @click="hidePassword = !hidePassword"></i>
          <input :type="passwordFieldType" id="password" v-model="password" placeholder="">

          <button type="submit">Log in</button>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { computed, ref } from "vue";
import axios from "../axios";
import router from "@/router";

const username = ref("");
const hidePassword = ref(true);
const password = ref("");

const passwordFieldIcon = computed(() => hidePassword.value ? "fa-eye" : "fa-eye-slash");
const passwordFieldType = computed(() => hidePassword.value ? "password" : "text");

const doLogin = async () => {
  try {
    const response = await axios.post('api/user/login', {
      username: username.value,
      password: password.value
    });

    localStorage.setItem('token', response.data.token);
    router.push({ name: 'TimeTrackerList' });
  }
  catch (error) {
    console.error("Erro ao fazer login: ", error);
    alert("Login falhou. Verifique suas credenciais  " + error.message);
  }
};
</script>

<style>
* {
  font-family: Verdana, sans-serif;
}

html,
body {
  height: 100%;
  margin: 0;
  padding: 0;
  width: 100%;
}

div#app {
  width: 100%;
  height: 100%;
}

div#app div#login {
  align-items: center;
  background-color: #e2e2e5;
  display: flex;
  flex-direction: column;
  justify-content: center;
  width: 100%;
  height: 100%;
}

div#app div#login div#description {
  background-color: #ed4d4dd9;
  color: #ffffff;
  width: 280px;
  padding: 35px;
  margin-bottom: 20px;
}

div#app div#login div#description h1,
div#app div#login div#description p {
  margin: 0;
}

div#app div#login div#description p {
  font-size: 0.8em;
  color: #95a5a6;
  margin-top: 10px;
}

div#app div#login div#form {
  background-color: #34495e;
  border-radius: 5px;
  box-shadow: 0px 0px 30px 0px #666;
  color: #ecf0f1;
  width: 300px;
  padding: 35px;
}

div#app div#login div#form label,
div#app div#login div#form input {
  outline: none;
  width: 100%;
}

div#app div#login div#form label {
  color: #95a5a6;
  font-size: 1em;
}

div#app div#login div#form input {
  background-color: transparent;
  color: #ecf0f1;
  font-size: 1em;
  margin-bottom: 20px;
  margin-top: 5px;
  border: solid 1.5px;
  height: 45px;
  padding: 10px;
  box-sizing: border-box;
}

div#app div#login div#form ::placeholder {
  color: #ecf0f1;
  opacity: 1;
}

div#app div#login div#form button {
  background-color: #ffffff;
  cursor: pointer;
  border: none;
  padding: 10px;
  height: 45px;
  transition: background-color 0.2s ease-in-out;
  width: 100%;
  box-sizing: border-box !important;
}

div#app div#login div#form button:hover {
  background-color: #ed4d4dd9;
}

@media screen and (max-width: 600px) {
  div#app div#login {
    align-items: unset;
    background-color: unset;
    display: unset;
    justify-content: unset;
  }

  div#app div#login div#description {
    margin: 0 auto;
    max-width: 350px;
    width: 100%;
  }

  div#app div#login div#form {
    border-radius: unset;
    box-shadow: unset;
    width: 100%;
  }

  div#app div#login div#form form {
    margin: 0 auto;
    max-width: 280px;
    width: 100%;
  }
}
</style>