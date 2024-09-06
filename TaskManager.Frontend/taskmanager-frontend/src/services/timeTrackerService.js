import axios from '../axios'; 

const TIME_TRACKER_API_URL = '/TimeTracker';
const TASK_API_URL = '/Task';
const COLLABORATOR_API_URL = '/Collaborator';

export const fetchTimeTrackers = async (params) => {
  try {
    const response = await axios.get(`${TIME_TRACKER_API_URL}/search`, { params });
    return response;
  } catch (error) {
    throw new Error(`Error fetching time trackers: ${error.message}`);
  }
};

export const deleteTimeTracker = async (id) => {
  try {
    await axios.delete(`${TIME_TRACKER_API_URL}/${id}`);
  } catch (error) {
    throw new Error(`Error deleting time tracker: ${error.message}`);
  }
};

export const saveTimeTracker = async (timeTracker) => {
  try {
    await axios.post(TIME_TRACKER_API_URL, timeTracker);
  } catch (error) {
    throw new Error(`Error saving time tracker: ${error.message}`);
  }
};

export const fetchTasks = async (params) => {
  try {
    const response = await axios.get(`${TASK_API_URL}/search`, { params });
    return response.data;
  } catch (error) {
    throw new Error(`Error fetching tasks: ${error.message}`);
  }
};

export const fetchCollaborators = async (params) => {
  try {
    const response = await axios.get(COLLABORATOR_API_URL, { params });
    return response.data;
  } catch (error) {
    throw new Error(`Error fetching collaborators: ${error.message}`);
  }
};