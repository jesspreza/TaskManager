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

    <!-- Formulário para TimeTracker -->
    <h6 class="cabecalho"><b>Inicie uma Tarefa</b></h6>
    <div class="form-widget">
      <b-form @submit.prevent="handleSubmit">
        <b-row class="align-items-center">
          <b-col md="5">
            <b-form-group label="Tarefa - Projeto *" label-class="bold-label" label-for="task-select"
              invalid-feedback="Selecione um projeto">
              <template v-slot:label>
                Tarefa - Projeto <span style="color: red;">*</span>
              </template>
              <b-form-select id="task-select" v-model="selectedTaskId" :options="taskOptions" :state="taskState"
                @input="validateTask" required></b-form-select>
            </b-form-group>
          </b-col>
          <b-col md="3">
            <b-form-group label="Colaborador *" label-class="bold-label" label-for="collaborator-select"
              invalid-feedback="Selecione um colaborador">
              <template v-slot:label>
                Colaborador <span style="color: red;">*</span>
              </template>
              <b-form-select id="collaborator-select" v-model="selectedCollaboratorId" :options="collaboratorOptions"
                :state="collaboratorState" @input="validateCollaborator" required></b-form-select>
            </b-form-group>
          </b-col>
          <b-col md="2">
            <b-form-group label="Tempo" label-class="bold-label">
              <p> {{ formattedTimespan() }}</p>
            </b-form-group>
          </b-col>
          <b-col md="2" class="row">
            <b-button @click="handleTimer" :class="isTracking ? 'btn btn-danger' : 'btn btn-info'">
              <i :class="isTracking ? 'fa fa-pause' : 'fa fa-play'"></i>
              {{ isTracking ? 'Parar' : 'Iniciar' }}
            </b-button>
          </b-col>
        </b-row>
      </b-form>
    </div>

    <!-- Grid para exibir TimeTrackers salvos -->
    <br>
    <h6 class="cabecalho"><b>Relação de Tarefas</b></h6>
    <div class="grid-widget">
      <div class="input-group m-2" style="float: right; width: 50%;">
        <input type="text" class="form-control" v-model="searchTerm"
          placeholder="Buscar tarefa, projeto ou colaborador..." />
        <span class="input-group-text">
          <i class="fas fa-filter"></i>
        </span>
        <b-form-select v-model="dateFilter" :options="dateOptions" class="ml-2"></b-form-select>
        <button id="searchbutton" class="btn btn-primary" @click="searchTimeTrackers" v-b-tooltip.hover title="Buscar">
          <i class="fas fa-search"></i>
        </button>
      </div>
      <b-table :items="timeTrackers" :fields="fields" striped hover :sort-by="sortBy" :sort-desc="sortDesc">
        <template v-slot:cell(taskName)="data">
          {{ data.item.taskResponseDto?.name + ' - ' + data.item.taskResponseDto.projectResponseDto.name || 'N/A' }}
        </template>
        <template v-slot:cell(timeZone)="data">
          {{ data.item.timeZoneId }}
        </template>
        <template v-slot:cell(collaborator)="data">
          {{ data.item.collaboratorResponseDto?.name || 'N/A' }}
        </template>
        <template v-slot:cell(startDate)="data">
          {{ formatDate(data.item.startDate) }}
        </template>
        <template v-slot:cell(endDate)="data">
          {{ formatDate(data.item.endDate) }}
        </template>
        <template v-slot:cell(timespan)="data">
          {{ formatTimespan(data.item.startDate, data.item.endDate) }}
        </template>
        <template v-slot:cell(actions)="data">
          <b-button :disabled="deletedItems(data.item)" @click="copyBaseInfo(data.item)" variant="info" class="btn-sm"
            style="margin-right: 3px;">
            <i class="fa fa-play"></i>
            Iniciar</b-button>
          <b-button @click="deleteTimeTracker(data.item)" variant="danger" class="btn-sm">
            <i class="fa fa-trash"></i>
          </b-button>
          <i v-if="deletedItems(data.item)" class="fa fa-circle-info" style="margin-left: 5px; color: #0d6efd;"
            v-b-tooltip.hover title="O Projeto, tarefa ou colaborador foi deletado"></i>
        </template>
      </b-table>
      <b-pagination v-model="paginationParams.pageNumber" :total-rows="totalRows" :per-page="paginationParams.pageSize"
        @page-click="onPageChange" class="my-1 pagination-container" style="float: right;"></b-pagination>
    </div>
  </div>
</template>

<script>
import axios from '../axios';
import { BButton, BForm, BFormGroup, BFormSelect, BTable, BRow, BCol, BPagination, BModal } from 'bootstrap-vue-3';

export default {
  name: 'TimeTrackerList',
  components: { BButton, BForm, BFormGroup, BFormSelect, BTable, BRow, BCol, BPagination, BModal },
  data() {
    return {
      paginationParams: {
        pageNumber: 1,
        pageSize: 10
      },
      searchTerm: '',
      dateFilter: '',
      dateOptions: [
        { value: '', text: 'Sem Filtro' },
        { value: 'today', text: 'Hoje' },
        { value: 'thisMonth', text: 'Este Mês' },
      ],
      taskOptions: [],
      collaboratorOptions: [],
      timeTrackers: [],
      selectedTaskId: null,
      selectedCollaboratorId: null,
      isTracking: false,
      startTime: null,
      timeZone: this.getTimeZone(),
      totalRows: 0,
      isModalVisible: false,
      isErrorModalVisible: false,
      errorMessage: '',
      itemToDelete: null,
      sortBy: 'StartDate',
      sortDesc: true,
      elapsedTime: 0,
      timerInterval: null,
      taskState: null,
      collaboratorState: null,
      fields: [
        { key: 'taskName', label: 'Tarefa - Projeto' },
        { key: 'timeZone', label: 'Fuso horário' },
        { key: 'collaborator', label: 'Colaborador' },
        { key: 'startDate', label: 'Início' },
        { key: 'endDate', label: 'Fim' },
        { key: 'timespan', label: 'Duração' },
        { key: 'actions', label: 'Ações' },
      ],
    };
  },
  computed: {
  },
  methods: {
    deletedItems(data) {
      var deleted = data.taskResponseDto?.deletedAt > new Date(0) || data.taskResponseDto?.deletedAt !== null ||
        data.taskResponseDto?.projectResponseDto?.deletedAt > new Date(0) || data.taskResponseDto?.projectResponseDto?.deletedAt !== null
      data.collaboratorResponseDto.deleteAt > new Date(0) || data.collaboratorResponseDto.deleteAt !== null;
      return deleted;
    },
    validateTask() {
      this.taskState = this.selectedTaskId ? true : false;
    },
    validateCollaborator() {
      this.collaboratorState = this.selectedCollaboratorId ? true : false;
    },
    deleteTimeTracker(item) {
      this.itemToDelete = item;
      this.isModalVisible = true;
    },
    async confirmDelete() {
      try {
        await axios.delete(`/TimeTracker/${this.itemToDelete.id}`);
        this.fetchTimeTrackers();
      } catch (error) {
        console.error('Erro ao excluir o rastreador de tempo: ', error);
        this.errorMessage = 'Erro ao deletar timeTracker: ' + error.message;
        this.isErrorModalVisible = true;
      } finally {
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
        this.fetchTimeTrackers();
      }
    },
    async searchTimeTrackers() {
      await this.fetchTimeTrackers();
    },
    async fetchTasks() {
      try {
        const response = await axios.get('/Task/search', {
          params: {
            searchTerm: this.searchTerm,
            pageNumber: this.paginationParams.pageNumber,
            pageSize: this.paginationParams.pageSize,
          }
        });
        this.taskOptions = [{ value: null, text: 'Selecione uma tarefa' }, ...response.data.map(task => ({
          value: task.id,
          text: task.name + ' - ' + task.projectName
        }))];
      } catch (error) {
        console.error('Error fetching tasks:', error);
        this.errorMessage = 'Erro ao buscar tarefas: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    async fetchCollaborators() {
      try {
        const response = await axios.get('/Collaborator', {
          params: {
            searchTerm: this.searchTerm,
            pageNumber: this.paginationParams.pageNumber,
            pageSize: this.paginationParams.pageSize,
          }
        });
        this.collaboratorOptions = [{ value: null, text: 'Selecione um colaborador' }, ...response.data.map(collaborator => ({
          value: collaborator.id,
          text: collaborator.name
        }))];
      } catch (error) {
        console.error('Error fetching collaborators:', error);
        this.errorMessage = 'Erro ao buscar colaboradores: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    async fetchTimeTrackers() {
      try {
        const response = await axios.get('/TimeTracker/search', {
          params: {
            searchTerm: this.searchTerm,
            dateFilter: this.dateFilter,
            pageNumber: this.paginationParams.pageNumber,
            pageSize: this.paginationParams.pageSize,
            sortBy: this.sortBy,
            sortDesc: this.sortDesc
          }
        });
        this.timeTrackers = response.data;
        const paginationHeader = response.headers['pagination'];
        if (paginationHeader) {
          const paginationData = JSON.parse(paginationHeader);
          this.paginationParams.pageNumber = paginationData.currentPage;
          this.paginationParams.pageSize = paginationData.itemsPerPage;
          this.totalRows = paginationData.totalItems;
        }
      } catch (error) {
        console.error('Error fetching time trackers:', error);
        this.errorMessage = 'Erro ao buscar rastreadores de tempo: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    formattedTimespan() {
      if (!this.isTracking || !this.startTime) return '00:00:00';
      const totalSeconds = Math.floor(this.elapsedTime / 1000);
      const hours = Math.floor(totalSeconds / 3600).toString().padStart(2, '0');
      const minutes = Math.floor((totalSeconds % 3600) / 60).toString().padStart(2, '0');
      const seconds = (totalSeconds % 60).toString().padStart(2, '0');
      return `${hours}:${minutes}:${seconds}`;
    },
    handleTimer() {
      this.validateTask();
      this.validateCollaborator();
      if (this.taskState && this.collaboratorState) {
        this.toggleTimer();
      } else {
        this.errorMessage = 'Selecione uma tarefa e um colaborador antes de iniciar.';
        this.isErrorModalVisible = true;
      }
    },
    toggleTimer() {
      if (this.isTracking) {
        this.stopTimer();
      } else if (this.taskState && this.collaboratorState) {
        this.startTimer();
      } else {
        this.errorMessage = 'Verifique os campos não preenchidos';
        this.isErrorModalVisible = true;
      }
    },
    startTimer() {
      this.startTime = new Date() - this.elapsedTime;
      this.isTracking = true;
      this.timerInterval = setInterval(() => {
        this.elapsedTime = Date.now() - this.startTime;
      }, 1000);
    },
    formatToLocalISOString(timestamp, timeZone) {
      const date = new Date(timestamp);

      const options = { timeZone, hour12: false, year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit', second: '2-digit' };

      const parts = new Intl.DateTimeFormat('en-CA', options).formatToParts(date).reduce((acc, part) => {
        acc[part.type] = part.value;
        return acc;
      }, {});

      return `${parts.year}-${parts.month}-${parts.day}T${parts.hour}:${parts.minute}:${parts.second}.${date.getMilliseconds().toString().padStart(3, '0')}`;
    },
    stopTimer() {
      clearInterval(this.timerInterval);

      const timeZone = this.timeZone.split(') ')[1].trim();
      try {
        Intl.DateTimeFormat(undefined, { timeZone }).format(new Date());
      } catch (e) {
        console.error(`Fuso horário inválido: ${timeZone}`);
        this.errorMessage = error.response?.data || `Fuso horário inválido: ${timeZone}`;
        this.isErrorModalVisible = true;
        return;
      }
      const formattedStartTime = this.formatToLocalISOString(this.startTime, timeZone)
      this.timerInterval = null;
      const endTime = new Date();
      const formattedEndTime = this.formatToLocalISOString(endTime.getTime(), timeZone);
      const timeTracker = {
        startDate: formattedStartTime,
        endDate: formattedEndTime,
        timeZoneId: this.timeZone,
        taskId: this.selectedTaskId,
        collaboratorId: this.selectedCollaboratorId,
      };
      this.saveTimeTracker(timeTracker);
      this.resetForm();
    },
    async saveTimeTracker(timeTracker) {
      try {
        await axios.post('/TimeTracker', timeTracker);
        this.fetchTimeTrackers();
        this.resetForm();
      } catch (error) {
        console.error('Error saving time tracker:', error);
        this.errorMessage = 'Erro ao salvar o timeTracker: ' + error.message;
        this.isErrorModalVisible = true;
      }
    },
    handleSubmit() {
      this.validateTask();
      this.validateCollaborator();
      if (this.taskState && this.collaboratorState) {
        this.handleSave();
      } else {
        this.errorMessage = 'Verifique os campos não preenchidos';
        this.isErrorModalVisible = true;
      }
    },
    async handleSave() {
      if (this.isTracking) {
        this.stopTimer();
      } else {
        this.startTimer();
      }
    },
    formatDate(date) {
      if (!date) return '';
      return new Date(date).toLocaleString();
    },
    formatTimespan(startDate, endDate) {
      if (!startDate || !endDate) return '00:00:00';
      const start = new Date(startDate);
      const end = new Date(endDate);
      const totalSeconds = Math.floor((end - start) / 1000);
      const hours = Math.floor(totalSeconds / 3600);
      const minutes = Math.floor((totalSeconds % 3600) / 60);
      const seconds = totalSeconds % 60;
      return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
    },
    copyBaseInfo(item) {
      this.selectedTaskId = item.taskId;
      this.selectedCollaboratorId = item.collaboratorId;
      this.timeZone = item.timeZoneId;
      this.startTime = null;
      this.elapsedTime = 0;
      this.isTracking = false;
      this.startTimer();
    },
    resetForm() {
      this.selectedTaskId = null;
      this.selectedCollaboratorId = null;
      this.isTracking = false;
      this.startTime = null;
      this.taskState = null;
      this.collaboratorState = null;
      clearInterval(this.timerInterval);
    },
    getTimeZone() {
      const timeZone = Intl.DateTimeFormat().resolvedOptions().timeZone;

      const offsetMinutes = new Date().getTimezoneOffset();
      const sign = offsetMinutes > 0 ? '-' : '+';
      const absOffset = Math.abs(offsetMinutes);
      const hours = String(Math.floor(absOffset / 60)).padStart(2, '0');
      const minutes = String(absOffset % 60).padStart(2, '0');
      const offsetFormatted = `${sign}${hours}:${minutes}`;

      return `(UTC ${offsetFormatted}) ${timeZone}`;
    }
  },
  mounted() {
    this.fetchTasks();
    this.fetchCollaborators();
    this.fetchTimeTrackers();
  },
};
</script>

<style>
.form-widget {
  margin-bottom: 20px !important;
  margin: 0px 0;
  padding: 20px;
  border: 1px solid #34495e;
  border-radius: 5px;
}

.grid-widget {
  margin-bottom: 20px !important;
  margin: 0px 0;
  padding: 20px;
  border: 1px solid #34495e;
  border-radius: 5px;
  font-size: small;
}

.grid-widget .form-group {
  display: flex;
  justify-content: flex-end;
  align-items: center;
}

.grid-widget .form-group .form-control {
  margin-right: 10px;
  width: auto;
}

.grid-widget .form-group .btn {
  margin-left: 10px;
}

.bold-label {
  font-weight: bold;
}

.b-modal .modal-content {
  border-radius: 8px;
}

.b-modal .modal-content {
  border-radius: 8px;
}

.cabecalho {
  border-radius: 5px;
  border: solid 1px #34495e;
  background-color: #34495e;
  color: #ffffff;
  line-height: 2em;
  margin: 0;
}

.b-modal .modal-body {
  color: #dc3545;
}

.confirm-delete .modal-title {
  color: #c0392b !important;
  font-size: 1.5rem;
  font-weight: bold;
}
.pagination-container {
  display: flex;
  justify-content: flex-end;
  margin: 20px 0;
}
</style>