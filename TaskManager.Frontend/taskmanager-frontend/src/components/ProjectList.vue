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

    <!-- Tabela de Projetos -->
    <div class="d-flex justify-content-end m-2">
      <div class="input-group m-2" style="float: right; width: 30%;">
        <input type="text" class="form-control" v-model="searchTerm" placeholder="Buscar projeto..." />
        <button class="btn btn-primary" @click="fetchProjects" v-b-tooltip.hover title="Buscar">
          <i class="fas fa-search"></i>
        </button>
      </div>
      <button class="btn btn-success m-2" @click="showProjectCreate">
        <i class="fas fa-plus"></i>
        Novo Projeto
      </button>
    </div>
    <div class="grid-widget">
      <table class="table b-table table-hover table-striped">
        <thead>
          <tr>
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
              Criado Em
              <a href="#" @click.prevent="sortTable('createdAt', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('createdAt', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Editado Em
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
          <tr v-for="project in sortedProjects" :key="project.id">
            <td>{{ project.name }}</td>
            <td>{{ formatDate(project.createdAt) }}</td>
            <td>{{ formatDate(project.updatedAt) }}</td>
            <td class="text-center" style="white-space: nowrap;">
              <b-button variant="primary" class="btn-sm" style="margin-right: 3px;"
                @click="showEditProjectModal(project)" v-b-tooltip.hover title="Editar">
                <i class="fas fa-edit"></i>
              </b-button>
              <b-button variant="danger" class="btn-sm" @click="deleteProject(project.id)" v-b-tooltip.hover
                title="Excluir">
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
      <!-- Modal para adicionar/editar projeto -->
      <b-modal id="projectCreateModal" :title="project.id ? 'Editar Projeto' : 'Novo Projeto'"
        v-model="showCreateProject" @ok="handleSubmit" @hide="closeModal">
        <b-form @submit.prevent="handleSubmit">
          <b-form-group label="Nome do Projeto" label-for="name-input"
            invalid-feedback="O nome é obrigatório e deve ter menos de 250 caracteres.">
            <b-form-input id="name-input" v-model="project.name" :state="nameState" @input="validateName" required
              placeholder="Digite o nome do projeto"></b-form-input>
          </b-form-group>
          <b-form-group>
            <b-button type="submit" variant="primary">Salvar</b-button>
          </b-form-group>
        </b-form>
      </b-modal>
    </div>
  </div>
</template>

<script>
import { projectService } from '@/services/projectService';
import { BButton, BModal, BForm, BFormGroup, BFormInput, BPagination } from 'bootstrap-vue-3';

export default {
  components: { BButton, BModal, BForm, BFormGroup, BFormInput, BPagination },
  data() {
    return {
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
      showCreateProject: false,
      showEditProject: false,
      isModalVisible: false,
      isErrorModalVisible: false,
      errorMessage: '',
      project: {
        id: null,
        name: '',
      },
      selectedProjectId: null,
      nameState: null,
    };
  },
  computed: {
    sortedProjects() {
      return this.projects.slice().sort((a, b) => {
        let modifier = this.sortOrder === 'asc' ? 1 : -1;
        if (a[this.sortKey] < b[this.sortKey]) return -1 * modifier;
        if (a[this.sortKey] > b[this.sortKey]) return 1 * modifier;
        return 0;
      });
    },
  },
  methods: {
    validateName() {
      this.nameState = this.project.name.length > 0 && this.project.name.length <= 250
    },
    deleteProject(item) {
      this.itemToDelete = item;
      this.isModalVisible = true;
    },
    async confirmDelete() {
      try {
        await projectService.deleteProject(this.itemToDelete);
        this.fetchProjects();
      } catch (error) {
        console.error('Error deleting project:', error);
        this.errorMessage = error.response?.data || 'Erro ao tentar deletar o projeto: ' + error.message;
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
        this.fetchProjects();
      }
    },
    sortTable(key, order) {
      this.sortKey = key;
      this.sortOrder = order;
    },
    showProjectCreate() {
      this.resetProjectForm();
      this.nameState = null;
      this.showCreateProject = true;
    },
    showEditProjectModal(project) {
      this.project = { id: project.id, name: project.name };
      this.showCreateProject = true;
    },
    closeModal() {
      this.resetProjectForm();
      this.showCreateProject = false;
    },
    async fetchProjects() {
      try {
        const response = await projectService.fetchProjects( 
          
            this.searchTerm,
            this.paginationParams.pageNumber,
            this.paginationParams.pageSize,
          
        );
        this.projects = response.data;
        const paginationHeader = response.headers['pagination'];
        if (paginationHeader) {
          const paginationData = JSON.parse(paginationHeader);
          this.paginationParams.pageNumber = paginationData.currentPage;
          this.paginationParams.pageSize = paginationData.itemsPerPage;
          this.totalRows = paginationData.totalItems;
        }
      } catch (error) {
        console.error('Error fetching projects:', error);
        this.errorMessage = error.response?.data || 'Erro ao buscar projetos: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    formatDate(date) {
      if (!date) return null;
      return new Date(date).toLocaleString();
    },
    handleSubmit() {
      this.validateName();
      if (this.nameState) {
        this.handleSave();
        console.log("Form is valid! Submitting...");
      } else {
        this.errorMessage = 'Verifique os campos não preenchidos';
        this.isErrorModalVisible = true;
      }
    },
    async handleSave() {
      try {
        if (this.project.id) {
          await projectService.updateProject(this.project.id, this.project);
          this.closeModal();
          this.fetchProjects();
        } else {
          await projectService.createProject(this.project);
          this.closeModal();
          this.fetchProjects();
        }
      } catch (error) {
        console.error('Error saving project:', error);
        this.errorMessage = error.response?.data || 'Erro ao salvar projeto: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    resetProjectForm() {
      this.project = {
        name: ''
      };
    },
  },

  mounted() {
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
  /* Cor para linhas ímpares */
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
}

.confirm-delete .modal-title {
  color: #c0392b !important;
  font-size: 1.5rem;
  font-weight: bold;
}
</style>