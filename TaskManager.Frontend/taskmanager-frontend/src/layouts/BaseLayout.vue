<template>
  <div>
    <!-- Cabeçalho -->
    <header>
      <nav>
        <ul>
          <li><router-link to="/timetrackers">Home</router-link></li>
          <li><router-link to="/tasks">Tarefas</router-link></li>
          <li><router-link to="/projects">Projetos</router-link></li>
          <li><router-link to="/tasks/report">Relatórios</router-link></li>

          <li class="logout-button">
            <button @click="logout">Sair</button>
          </li>
        </ul>
      </nav>
    </header>

    <!-- Conteúdo dinâmico -->
    <main>
      <router-view></router-view>
    </main>

    <!-- Rodapé -->
    <footer>
      <p>&copy; 2024 Task Manager</p>
      <small>Developed by Jessica Preza</small>
    </footer>
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router';

const router = useRouter();

const logout = () => {
  localStorage.removeItem('token');
  router.push('/login');
};
</script>


<style scoped>
header {
  background-color: #34495e;
  padding: 10px;
  color: white;
}

header nav ul {
  list-style: none;
  margin: 0;
  padding: 0;
  display: flex;
  justify-content: space-around;
  align-items: center;
}

header nav ul li {
  margin: 0 10px;
  display: flex;
  align-items: center;
}

header nav ul li a {
  color: white;
  text-decoration: none;
  padding: 10px;
  transition: background-color 0.3s, color 0.3s;
}

header nav ul li .router-link-active {
  background-color: #ed4d4dd9;
  border-radius: 4px;
}

header nav ul li a:hover {
  background-color: rgba(237, 77, 77, 0.4);
  border-radius: 4px;
}

main {
  padding: 20px;
  min-height: calc(100vh - 190px);
  box-sizing: border-box;
  flex-direction: column;
  justify-content: center;
}

footer {
  background-color: #34495e;
  color: white;
  text-align: center;
  padding: 10px;
  position: relative;
  bottom: 0;
  width: 100%;
  box-sizing: border-box;
}

.logout-button {
  margin-left: auto;
}

.logout-button button {
  background-color: #e74c3c;
  border: none;
  color: white;
  padding: 10px 15px;
  cursor: pointer;
  border-radius: 4px;
  transition: background-color 0.3s;
  align-self: center;
}

.logout-button button:hover {
  background-color: #c0392b;
}

.grid-widget {
  margin: 20px 0;
  padding: 20px;
  border: 1px solid #34495e;
  border-radius: 5px;
  font-size: small;
}

.error-modal .modal-content {
  z-index: 1050 !important;
  /* Ajuste conforme necessário */
}

.error-modal .modal-title {
  color: #c0392b;
  font-size: 1.5rem;
  font-weight: bold;
  /* Defina a cor desejada para o título do modal */
}
</style>