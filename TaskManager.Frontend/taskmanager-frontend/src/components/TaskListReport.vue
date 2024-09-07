<template>
  <div>
    <!-- Modal de Erro -->
    <modal-error
      :show="isErrorModalVisible"
      :errorMessage="errorMessage"
    />

    <div class="d-flex justify-content-end m-2">
      <div class="input-group m-2" style="float: right; width: 30%;">
        <input type="text" class="form-control" v-model="searchTerm" @input="onSearchTermChange"
          placeholder="Buscar tarefa ou projeto..." />
        <button class="btn btn-primary" @click="filterTasks" v-b-tooltip.hover title="Buscar">
          <i class="fas fa-search"></i>
        </button>
      </div>
      <div class="input-group m-2" style="float: right; width: 20%;">
          <span class="input-group-text">
            <i class="fas fa-filter"></i>
          </span>
        <!-- Select para colaboradores -->
        <select v-model="selectedCollaborator" class="form-control" @change="onCollaboratorChange">
          <option value="">Todos os Colaboradores</option>
          <option v-for="collaborator in collaboratorOptions" :key="collaborator.name" :value="collaborator.name">
            {{ collaborator.name }}
          </option>
        </select>
      </div>
    </div>
    <div class="grid-widget">
      <table class='table b-table table-hover table-striped'>
        <thead>
          <tr>
            <th>
              Tarefa
              <a href="#" @click.prevent="sortTable('task.name', 'asc')" v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('task.name', 'desc')" v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Projeto
              <a href="#" @click.prevent="sortTable('task.projectName', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('task.projectName', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th>
              Tempo/Tarefa/Dia
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
              Tempo/Tarefa/Mês
              <a href="#" @click.prevent="sortTable('task.timePerMonthPerTask', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortTable('task.timePerMonthPerTask', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a>
            </th>
            <th class="collaborator">
              Colaborador
              <!-- <a href="#" @click.prevent="sortInternal('tracker.collaboratorName', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortInternal('tracker.collaboratorName', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a> -->
            </th>
            <th class="time-tracker">
              DataHora Início
              <!-- <a href="#" @click.prevent="sortInternal('tracker.startDate', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortInternal('tracker.startDate', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a> -->
            </th>
            <th class="time-tracker">
              DataHora Fim
              <!-- <a href="#" @click.prevent="sortInternal('tracker.endDate', 'asc')" v-b-tooltip.hover
                title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortInternal('tracker.endDate', 'desc')" v-b-tooltip.hover
                title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a> -->
            </th>
            <th class="per-day">
              Tempo/Colaborador/Dia
              <!-- <a href="#" @click.prevent="sortInternal('collaboratorGroup.timePerDayPerCollaborator', 'asc')"
                v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a> -->
              <!-- <a href="#" @click.prevent="sortInternal('collaboratorGroup.timePerDayPerCollaborator', 'desc')"
                v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a> -->
            </th>
            <th class="per-month">
              Tempo/Colaborador/Mês
              <!-- <a href="#" @click.prevent="sortInternal('collaboratorGroup.timePerMonthPerCollaborator', 'asc')"
                v-b-tooltip.hover title="Ordenar Ascendente">
                <i class="fas fa-sort-up"></i>
              </a>
              <a href="#" @click.prevent="sortInternal('collaboratorGroup.timePerMonthPerCollaborator', 'desc')"
                v-b-tooltip.hover title="Ordenar Descendente">
                <i class="fas fa-sort-down"></i>
              </a> -->
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-for="task in sortedTasks" :key="task.id">
            <tr>
              <td>
                <button @click="toggleExpand(task.id)" class="btn btn-link">
                  <i :class="expandedTaskId === task.id ? 'fas fa-chevron-up' : 'fas fa-chevron-down'"></i>
                </button>
                {{ task.name }}
              </td>
              <td>{{ task.projectName }}</td>
              <td>{{ formatTime(task.timePerDayPerTask) || 'N/A' }}</td>
              <td>{{ formatTime(task.timePerMonthPerTask) || 'N/A' }}</td>
              <td :class="{
                'text-muted': !hasTimeTrackers(task.id) || expandedTaskId !== task.id,
                'text-detalhe': expandedTaskId === task.id
              }" colspan="5">
                <template v-if="!hasTimeTrackers(task.id)">
                  Nenhuma atividade
                </template>
                <template v-else-if="expandedTaskId !== task.id">
                  Expanda para detalhes
                </template>
                <template v-else>
                  DETALHES DAS ATIVIDADES
                </template>
              </td>
            </tr>
            <template v-if="expandedTaskId === task.id">
              <template v-for="collaboratorGroup in groupTrackersByCollaborator(task.timeTrackerReportResponseDto)"
                :key="collaboratorGroup.collaboratorName">
                <!-- Linha para o total de tempo do colaborador -->
                <tr>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td class="collaborator">{{ collaboratorGroup.collaboratorName }}</td>
                  <td></td>
                  <td></td>
                  <td class="per-day">{{ formatTime(collaboratorGroup.timePerDayPerCollaborator) || 'N/A' }}</td>
                  <td class="per-month">{{ formatTime(collaboratorGroup.timePerMonthPerCollaborator) || 'N/A' }}</td>
                </tr>
                <!-- Linhas para cada TimeTracker do colaborador -->
                <tr v-for="tracker in collaboratorGroup.trackers" :key="tracker.timeTrackerId">
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td></td>
                  <td class="time-tracker">{{ formatDate(tracker.startDate) || 'N/A' }}</td>
                  <td class="time-tracker">{{ formatDate(tracker.endDate) || 'N/A' }}</td>
                  <td></td>
                  <td></td>
                </tr>
              </template>
            </template>
          </template>
        </tbody>
      </table>
      <b-pagination v-model="paginationParams.pageNumber" :total-rows="totalRows" :per-page="paginationParams.pageSize"
        @page-click="onPageChange" class="my-1" style="float: right;"></b-pagination>
    </div>
  </div>
</template>

<script>
import ModalError from './ModalError.vue';
import taskService from '@/services/taskService';
import collaboratorService from '@/services/collaboratorService';
import { BPagination } from 'bootstrap-vue-3';

export default {
  components: { BPagination, ModalError },
  data() {
    return {
      tasks: [],
      selectedProject: '',
      selectedCollaborator: '',
      searchTerm: '',
      sortKey: 'task.name',
      sortOrder: 'asc',
      internalSortKey: 'tracker.collaboratorName',
      internalSortOrder: 'asc',
      expandedTaskId: null,
      paginationParams: {
        pageNumber: 1,
        pageSize: 10
      },
      totalRows: 0,
      showCreateTask: false,
      showEditTask: false,
      selectedTask: null,
      showAddTaskToProjectModal: false,
      task: {
        name: '',
        description: '',
        projectId: null
      },
      projectOptions: [],
      taskOptions: [],
      collaboratorOptions: [],
      selectedTaskId: null,
      allTasks: [],
      isErrorModalVisible: false,
    };
  },
  computed: {
    sortedTasks() {
      if (!Array.isArray(this.tasks)) {
        return [];
      }
      return this.tasks
        .filter(task =>
          task.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          task.projectName.toLowerCase().includes(this.searchTerm.toLowerCase())
        )
        .sort((a, b) => {
          if (this.sortKey === 'task.name') {
            return this.sortOrder === 'asc'
              ? a.name.localeCompare(b.name)
              : b.name.localeCompare(a.name);
          }
          if (this.sortKey === 'task.projectName') {
            return this.sortOrder === 'asc'
              ? a.projectName.localeCompare(b.projectName)
              : b.projectName.localeCompare(a.projectName);
          }
          if (this.sortKey === 'task.timePerDayPerTask') {
            return this.sortOrder === 'asc'
              ? (a.timePerDayPerTask || 0) - (b.timePerDayPerTask || 0)
              : (b.timePerDayPerTask || 0) - (a.timePerDayPerTask || 0);
          }
          if (this.sortKey === 'task.timePerMonthPerTask') {
            return this.sortOrder === 'asc'
              ? (a.timePerMonthPerTask || 0) - (b.timePerMonthPerTask || 0)
              : (b.timePerMonthPerTask || 0) - (a.timePerMonthPerTask || 0);
          }
          return 0;
        });
    },
    sortedTrackers() {
      return (trackers) => {
        if (!Array.isArray(trackers)) {
          return [];
        }
        return trackers.sort((a, b) => {
          if (this.internalSortKey === 'tracker.collaboratorName') {
            return this.internalSortOrder === 'asc'
              ? a.collaboratorName.localeCompare(b.collaboratorName)
              : b.collaboratorName.localeCompare(a.collaboratorName);
          }
          if (this.internalSortKey === 'tracker.startDate') {
            return this.internalSortOrder === 'asc'
              ? new Date(a.startDate) - new Date(b.startDate)
              : new Date(b.startDate) - new Date(a.startDate);
          }
          if (this.internalSortKey === 'tracker.endDate') {
            return this.internalSortOrder === 'asc'
              ? new Date(a.endDate) - new Date(b.endDate)
              : new Date(b.endDate) - new Date(a.endDate);
          }
          return 0;
        });
      };
    }
  },
  methods: {
    groupTrackersByCollaborator(trackers) {
      const grouped = {};

      const filteredTrackers = this.selectedCollaborator
        ? trackers.filter(tracker => tracker.collaboratorName === this.selectedCollaborator)
        : trackers;

      filteredTrackers.forEach(tracker => {
        if (!grouped[tracker.collaboratorName]) {
          grouped[tracker.collaboratorName] = {
            collaboratorName: tracker.collaboratorName,
            timePerDayPerCollaborator: tracker.timePerDayPerCollaborator,
            timePerMonthPerCollaborator: tracker.timePerMonthPerCollaborator,
            trackers: []
          };
        }
        grouped[tracker.collaboratorName].timePerDayPerCollaborator || 0;
        grouped[tracker.collaboratorName].timePerMonthPerCollaborator || 0;
        grouped[tracker.collaboratorName].trackers.push(tracker);
      });

      return Object.values(grouped);
    },
    async fetchTasks() {
      try {
        const { tasks, pagination } = await taskService.fetchTasks( {
          searchTerm: this.searchTerm,
            pageNumber: this.paginationParams.pageNumber,
            pageSize: this.paginationParams.pageSize,
        });
        this.tasks = tasks;
        this.allTasks = tasks;
        this.filterTasks();
        this.paginationParams.pageNumber = pagination.currentPage || this.paginationParams.pageNumber;
        this.paginationParams.pageSize = pagination.itemsPerPage || this.paginationParams.pageSize;
        this.totalRows = pagination.totalItems || 0;
        
      } catch (error) {
        console.error('Error fetching tasks:', error);
        this.errorMessage = 'Erro ao tentar buscar tarefas: ' + error.message;
        this.isErrorModalVisible = true;
      }
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
    formatTime(timeSpan) {
      if (!timeSpan) return '00:00:00';
      const [hours, minutes, seconds] = timeSpan.split(':');
      const formattedSeconds = seconds.split('.')[0];
      return `${hours.padStart(2, '0')}:${minutes.padStart(2, '0')}:${formattedSeconds.padStart(2, '0')}`;
    },
    formatDate(date) {
      if (!date) return 'N/A';
      return new Date(date).toLocaleString();
    },
    toggleExpand(taskId) {
      this.expandedTaskId = this.expandedTaskId === taskId ? null : taskId;
    },
    sortTable(key, order) {
      this.sortKey = key;
      this.sortOrder = order;
      this.fetchTasks();
    },
    sortInternal(key, order) {
      this.internalSortKey = key;
      this.internalSortOrder = order;
    },
    hasTimeTrackers(taskId) {
      const task = this.tasks.find(t => t.id === taskId);
      return task && task.timeTrackerReportResponseDto && task.timeTrackerReportResponseDto.length > 0;
    },
    async fetchCollaborators() {
      try {
        const response = await collaboratorService.fetchCollaborators({
          pageNumber: this.paginationParams.pageNumber,
          pageSize: this.paginationParams.pageSize
        });
        this.collaboratorOptions = response.data;
      }
      catch (error) {
        this.errorMessage = 'Erro ao tentar buscar colaboradores: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    filterTasks() {
      this.tasks = this.allTasks.filter(task => {
        const matchesSearchTerm = task.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
          task.projectName.toLowerCase().includes(this.searchTerm.toLowerCase());

        const matchesCollaborator = this.selectedCollaborator === '' || task.timeTrackerReportResponseDto.some(t => t.collaboratorName === this.selectedCollaborator);
        return matchesSearchTerm && matchesCollaborator;
      });
    },
    onSearchTermChange() {
      this.filterTasks(); // Filtrar sempre que o termo de busca mudar
    },
    onCollaboratorChange() {
      this.filterTasks(); // Filtrar sempre que o colaborador selecionado mudar
    }
  },
  mounted() {
    this.fetchTasks();
    this.fetchCollaborators();
  }
}
</script>

<style>

.text-muted {
  color: #6c757d;
}

.text-detalhe {
  color: #4f5052 !important;
  text-align: center !important;
}

.collaborator {
  background-color: rgb(183, 198, 226) !important;
}

.time-tracker {
  background-color: rgb(235, 205, 167) !important;
}

.per-day {
  background-color: rgb(201, 233, 165) !important;
}

.per-month {
  background-color: rgb(228, 177, 177) !important;
}
</style>