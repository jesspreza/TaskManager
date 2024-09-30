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
import '../assets/styles/global.css'
import { useAuth } from '../composables/useAuth'

const { logout } = useAuth(); 
</script>