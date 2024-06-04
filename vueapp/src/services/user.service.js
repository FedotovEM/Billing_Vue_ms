import axios from 'axios';
import authHeader from './auth-header';
import { urls } from '../settings.js';

class UserService {
    getPublicContent() {
        return axios.get(urls + '/authorization/all');
    }

    getUserBoard() {
        return axios.get(urls + '/authorization/user', { headers: authHeader() });
    }

    getModeratorBoard() {
        return axios.get(urls + '/authorization/mod', { headers: authHeader() });
    }

    getAdminBoard() {
        return axios.get(urls + '/authorization/admin', { headers: authHeader() });
    }
}

export default new UserService();