<template>
  <div>
    <!-- Modal confirmação -->
    <b-modal class="confirm-delete" v-model="isModalVisible" title="Confirmação" @ok="confirmDelete"
      @cancel="cancelDelete" ok-title="Sim" cancel-title="Cancelar" ok-variant="danger">
      <p>Você tem certeza que deseja excluir este item?</p>
    </b-modal>

    <!-- Modal de Erro -->
    <b-modal class="error-modal" v-model="isErrorModalVisible" title="Erro" hide-footer>
      <p>{{ errorMessage }}</p>
    </b-modal>
    
    <!-- Tabela de Tasks -->
    <div class="d-flex justify-content-end m-2">
      <div class="input-group m-2" style="float: right; width: 30%;">
        <input type="text" class="form-control" v-model="searchTerm" placeholder="Buscar tarefa..." />
        <button class="btn btn-primary" @click="fetchTasks" v-b-tooltip.hover title="Buscar">
          <i class="fas fa-search"></i>
        </button>
      </div>
      <button class="btn btn-success m-2" @click="showTaskCreate">
        <i class="fas fa-plus"></i>
        Nova Tarefa
      </button>
    </div>
    <div class="grid-widget">
      <table class="table b-table table-hover table-striped">
        <thead>
          <tr>
            <th>
              Tarefa
              <a href="#" @click.prevent="sortTable('name', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('name', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Descrição
              <a href="#" @click.prevent="sortTable('name', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('name', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Projeto
              <a href="#" @click.prevent="sortTable('name', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('name', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Tempo Gasto/Dia
              <a href="#" @click.prevent="sortTable('task.timePerDayPerTask', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('task.timePerDayPerTask', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Tempo Gasto/Mês
              <a href="#" @click.prevent="sortTable('task.timePerMonthPerTask', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('task.timePerMonthPerTask', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Criada Em
              <a href="#" @click.prevent="sortTable('createdAt', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('createdAt', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Editada Em
              <a href="#" @click.prevent="sortTable('updatedAt', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('updatedAt', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th class="text-center" style="white-space: nowrap;">Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="task in sortedTasks" :key="task.id">
            <td>{{ task.name }}</td>
            <td>{{ task.description }}</td>
            <td>{{ task.projectName }}</td>
            <td>{{ formatTime(task.timePerDayPerTask) || 'N/A' }}</td>
            <td>{{ formatTime(task.timePerMonthPerTask) || 'N/A' }}</td>
            <td>{{ formatDate(task.createdAt) }}</td>
            <td>{{ formatDate(task.updatedAt) }}</td>
            <td class="text-center" style="white-space: nowrap;">
              <b-button variant="primary" class="btn-sm" style="margin-right: 3px;" @click="showEditTaskModal(task)"
                v-b-tooltip.hover title="Editar">
                <i class="fas fa-edit"></i>
              </b-button>
              <b-button variant="danger" class="btn-sm" @click="deleteTask(task.id)" v-b-tooltip.hover title="Excluir">
                <i class="fas fa-trash-alt"></i>
              </b-button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <b-pagination v-model="paginationParams.pageNumber" :total-rows="totalRows" :per-page="paginationParams.pageSize"
      @page-click="onPageChange" class="my-1" style="float: right;"></b-pagination>

    <div>
      <!-- Modal para adicionar/editar tarefa -->
      <b-modal id="task-modal" :title="task.id ? 'Editar Tarefa' : 'Nova Tarefa'" ref="taskForm"
        v-model="showCreateTask" @ok="handleSubmit">
        <b-form @submit.stop.prevent="handleSubmit">
          <b-form-group label="Nome da Tarefa" label-for="name-input"
            invalid-feedback="O nome é obrigatório e deve ter menos de 250 caracteres.">
            <b-form-input id="name-input" v-model="task.name" :state="nameState" @input="validateName" required
              maxlength="250" placeholder="Digite o nome da tarefa"></b-form-input>
          </b-form-group>

          <b-form-group label="Descrição" label-for="description-input"
            invalid-feedback="A descrição deve ter menos de 500 caracteres">
            <b-form-textarea id="description-input" v-model="task.description" :state="descriptionState"
              @input="validateDescription" placeholder="Digite a descrição (optional)"></b-form-textarea>
          </b-form-group>

          <b-form-group label="Projeto associado" label-for="project-select"
            invalid-feedback="Por favor, selecione um projeto.">
            <b-form-select id="project-select" v-model="task.projectId" :state="projectState" @input="validateProject"
              :options="projectOptions" required></b-form-select>
          </b-form-group>
          <b-form-group>
            <b-button type="submit" variant="primary">Salvar</b-button>
          </b-form-group>
        </b-form>
      </b-modal>

      <!-- Modal para adicionar tarefas a um projeto -->
      <b-modal id="addTaskToProjectModal" title="Adicionar Tarefa ao Projeto" v-model="showAddTaskToProject"
        @ok="handleAddTaskToProject" @hide="resetTaskToProjectForm">
        <b-form @submit.prevent="handleAddTaskToProject">
          <b-form-group label="Tarefa">
            <b-form-select v-model="selectedTaskId" :options="taskOptions"></b-form-select>
          </b-form-group>
          <b-form-group>
            <b-button type="submit" variant="primary">Adicionar</b-button>
          </b-form-group>
        </b-form>
      </b-modal>
    </div>
  </div>
</template>

<script>
import taskService from '@/services/taskService';
import { BButton, BModal, BForm, BFormGroup, BFormInput, BPagination, BFormSelect, BFormTextarea } from 'bootstrap-vue-3';

export default {
  components: { BButton, BModal, BForm, BFormGroup, BFormInput, BPagination, BFormSelect, BFormTextarea },
  data() {
    return {
      tasks: [],
      projects: [],
      selectedProject: '',
      searchTerm: '',
      sortKey: '',
      sortOrder: '',
      paginationParams: {
        pageNumber: 1,
        pageSize: 10
      },
      totalRows: 0,
      showCreateTask: false,
      showEditTask: false,
      isModalVisible: false,
      isErrorModalVisible: false,
      errorMessage: '',
      project: {
        id: null,
        name: '',
      }, task: {
        name: '',
        description: '',
        projectId: null
      },
      selectedProjectId: null,
      selectedTaskId: null,
      nameState: null,
      descriptionState: null,
      projectState: null,
    };
  },
  computed: {
    sortedTasks() {
      return this.tasks.slice().sort((a, b) => {
        let modifier = this.sortOrder === 'asc' ? 1 : -1;
        if (a[this.sortKey] < b[this.sortKey]) return -1 * modifier;
        if (a[this.sortKey] > b[this.sortKey]) return 1 * modifier;
        return 0;
      });
    },
  },
  methods: {
    validateName() {
      this.nameState = this.task.name.length > 0 && this.task.name.length <= 250
    },
    validateDescription() {
      this.descriptionState = this.task.description.length <= 500
    },
    validateProject() {
      this.projectState = this.task.projectId !== null;
    },
    formatTime(timeSpan) {
      const [hours, minutes, seconds] = timeSpan.split(':');
      const formattedSeconds = seconds.split('.')[0];
      return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:${formattedSeconds.padStart(2, '0')}`;
    },
    formatDate(date) {
      if (!date) return null;
      return new Date(date).toLocaleString();
    },
    deleteTask(item) {
      this.itemToDelete = item;
      this.isModalVisible = true;
    },
    async confirmDelete() {
      try {
        await taskService.deleteTask(this.itemToDelete);
        this.fetchTasks(); // Refresh the list
      } catch (error) {
        console.error('Error deleting task:', error);
        this.errorMessage = 'Erro ao tentar deletar a tarefa: ' + error.message;
        this.isErrorModalVisible = true;
      }
      finally {
        this.isModalVisible = false;
        this.itemToDelete = null;
      }
    },
    cancelDelete() {
      this.isModalVisible = false;
      this.itemToDelete = null;
    },
    onPageChange(event) {
      let newPage;

      if (typeof event === 'number') {
        newPage = event;
      } else if (event && event.target) {
        if (event.target.classList.contains('page-link')) {
          const pageText = event.target.innerText.trim();

          if (pageText === '‹') {
            newPage = this.paginationParams.pageNumber - 1;
          } else if (pageText === '›') {
            newPage = this.paginationParams.pageNumber + 1;
          } else if (pageText === '«') {
            newPage = 1;
          } else if (pageText === '»') {
            newPage = Math.ceil(this.totalRows / this.paginationParams.pageSize);
          } else {
            newPage = parseInt(pageText, 10);
          }
        }
      }

      if (newPage && !isNaN(newPage) && newPage > 0) {
        this.paginationParams.pageNumber = newPage;
        this.fetchTasks();
      }
    },
    sortTable(key, order) {
      this.sortKey = key;
      this.sortOrder = order;
    },
    showTaskCreate() {
      this.resetTaskForm();
      this.showCreateTask = true;
    },
    showEditTaskModal(task) {
      this.task = { id: task.id, name: task.name, description: task.description, projectId: task.projectId };
      this.showCreateTask = true;
    },
    closeModal() {
      this.resetTaskForm();
      this.resetTaskToProjectForm();
      this.showCreateTask = false;
    },
    showAddTaskToProject() {
      this.showAddTaskToProjectModal = true;
    },
    async handleAddTaskToProject() {
      try {
        await taskService.addTaskToProject(this.task.projectId, this.selectedTaskId);
        this.fetchProjects();
        this.showAddTaskToProjectModal = false;
      } catch (error) {
        console.error('Error adding task to project:', error);
      }
    },
    async fetchTasks() {
      try {
        const  { tasks, pagination } = await taskService.fetchTasks({
          
            searchTerm: this.searchTerm,
            pageNumber: this.paginationParams.pageNumber,
            pageSize: this.paginationParams.pageSize,
        });
        this.tasks = tasks;
        this.paginationParams.pageNumber = pagination.currentPage || this.paginationParams.pageNumber;
        this.paginationParams.pageSize = pagination.itemsPerPage || this.paginationParams.pageSize;
        this.totalRows = pagination.totalItems || 0;
        
      } catch (error) {
        console.error('Error fetching tasks:', error);
        this.errorMessage = 'Erro ao buscar tarefas: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    async fetchProjects() {
      try {
        this.projectOptions = await taskService.fetchProjects({
          pageNumber: this.paginationParams.pageNumber,
          pageSize: this.paginationParams.pageSize,
        });
      } catch (error) {
        console.error('Error fetching projects:', error);
      }
    },
    handleSubmit() {
      this.validateName();
      this.validateDescription();
      this.validateProject();
      if (this.nameState && this.descriptionState && this.projectState) {
        this.handleSave();
        console.log("Form is valid! Submitting...");
      } else {
        this.errorMessage = 'Verifique os campos ';
        this.isErrorModalVisible = true;
      }
    },
    async handleSave() {
      try {
        await taskService.saveTask(this.task);
        this.closeModal();
        this.fetchTasks();
      } catch (error) {
        console.error('Error saving task:', error);
        this.errorMessage = 'Erro ao salvar tarefa: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    resetTaskForm() {
      this.task = {
        name: '',
        description: '',
        projectId: null,
      };
      this.$refs.taskForm.reset;
    },
    resetTaskToProjectForm() {
      this.selectedTaskId = null;
    }
  },

  mounted() {
    this.fetchTasks();
    this.fetchProjects();
  },

};
</script>

<style>
table {
  width: 100%;
  border-collapse: collapse;
  border: 1px solid #afafaf;
}

th,
td {
  border: 1px solid #ddd;
  padding: 8px;
  text-align: left;
}

th {
  background-color: #f4f4f4;
}

tbody,
td,
tfoot,
th,
thead,
tr {
  border: 1px solid #afafaf !important;
}

tbody tr:hover {
  background-color: #e0e0e0 !important;
}

tbody tr:nth-child(odd) {
  background-color: #f9f9f9;
}

tbody tr:nth-child(even) {
  background-color: #ffffff;
}

.fa-sort-up,
.fa-sort-down {
  font-size: 0.75rem;
  cursor: pointer;
  margin-left: 5px;
}

.b-modal .modal-content {
  border-radius: 8px;
}

.b-modal .modal-content {
  border-radius: 8px;
}

.b-modal .modal-body {
  color: #dc3545;
  /* Cor do texto de erro */
}

.confirm-delete .modal-title {
  color: #c0392b !important;
  font-size: 1.5rem;
  font-weight: bold;
}
</style>