import axios from 'axios';
import { urls } from '../settings.js';

class AuthService {
    login(user) {
        return axios
            .post(urls.authServ + '/login', {
                username: user.username,
                password: user.password
            })
            .then(response => {
                if (response.data.token) {
                    localStorage.setItem('user', JSON.stringify(response.data));
                }

                return response.data;
            });
    }

    logout() {
        localStorage.removeItem('user');
    }

    register(user) {
        return axios.post(urls.authServ + '/register', {
            username: user.username,
            email: user.email,
            password: user.password
        });
    }
}

export default new AuthService();