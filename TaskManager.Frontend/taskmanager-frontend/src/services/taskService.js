import axios from '../axios';

const API_URL = '/Task';
const PROJECT_API_URL = '/Project';

const taskService = {
  fetchTasks(params) {
    return axios.get(`${API_URL}/search`, { params })
      .then(response => {
        const tasks = response.data;
        const paginationHeader = response.headers['pagination'];
        let pagination = {};

        if (paginationHeader) {
          pagination = JSON.parse(paginationHeader);
        }

        return {
          tasks,
          pagination
        };
      })
      .catch(error => {
        console.error('Error fetching tasks:', error);
        throw new Error('Erro ao buscar tarefas: ' + error.message);
      });
  },

  fetchProjects(params) {
    return axios.get(PROJECT_API_URL, { params })
      .then(response => {
        const projects = response.data;
        return projects.map(project => ({
          value: project.id,
          text: project.name
        }));
      })
      .catch(error => {
        console.error('Error fetching projects:', error);
        throw new Error('Erro ao buscar projetos: ' + error.message);
      });
  },

  // Create or update a task
  saveTask(task) {
    if (task.id) {
      return axios.put(`${API_URL}/${task.id}`, task)
        .catch(error => {
          console.error('Error updating task:', error);
          throw new Error('Erro ao atualizar tarefa: ' + error.message);
        });
    } else {
      return axios.post(API_URL, task)
        .catch(error => {
          console.error('Error creating task:', error);
          throw new Error('Erro ao criar tarefa: ' + error.message);
        });
    }
  },

  // Delete a task
  deleteTask(taskId) {
    return axios.delete(`${API_URL}/${taskId}`)
      .catch(error => {
        console.error('Error deleting task:', error);
        throw new Error('Erro ao deletar tarefa: ' + error.message);
      });
  },

  // Add task to project
  addTaskToProject(projectId, taskId) {
    return axios.post(`${PROJECT_API_URL}/${projectId}/Tasks`, { taskId })
      .catch(error => {
        console.error('Error adding task to project:', error);
        throw new Error('Erro ao adicionar tarefa ao projeto: ' + error.message);
      });
  }
};

export default taskService;