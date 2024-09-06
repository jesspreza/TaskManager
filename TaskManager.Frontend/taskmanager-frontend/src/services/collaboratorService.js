import axios from '../axios'; // Ajuste o caminho conforme necessário

const collaboratorService = {
  async fetchCollaborators(params) {
    try {
      const response = await axios.get('/Collaborator', { params });
      return response;
    } catch (error) {
      throw new Error('Error fetching collaborators: ' + error.message);
    }
  }
};

export default collaboratorService;