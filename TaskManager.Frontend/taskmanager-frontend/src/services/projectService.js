import axios from '../axios';

export const projectService = {
  async fetchProjects(searchTerm, pageNumber, pageSize) {
    try {
      const response = await axios.get('/Project/search', {
        params: {
          searchTerm,
          pageNumber,
          pageSize,
        },
      });
      return response;
    } catch (error) {
      throw new Error(error.response?.data || 'Erro ao buscar projetos: ' + error.message);
    }
  },

  async createProject(project) {
    try {
      const response = await axios.post('/project', project);
      return response.data;
    } catch (error) {
      throw new Error(error.response?.data || 'Erro ao criar projeto: ' + error.message);
    }
  },

  async updateProject(id, project) {
    try {
      const response = await axios.put(`/project/${id}`, project);
      return response.data;
    } catch (error) {
      throw new Error(error.response?.data || 'Erro ao atualizar projeto: ' + error.message);
    }
  },

  async deleteProject(id) {
    try {
      await axios.delete(`/project/${id}`);
    } catch (error) {
      throw new Error(error.response?.data || 'Erro ao deletar projeto: ' + error.message);
    }
  },
};
