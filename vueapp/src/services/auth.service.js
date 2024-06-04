import axios from 'axios';
import { urls } from '../settings.js';

class AuthService {
    login(user) {
        return axios
            .post(urls.authServ + '/authorization/login', {
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
        return axios.post(urls.authServ + '/authorization/register', {
            username: user.username,
            email: user.email,
            password: user.password
        });
    }
}

export default new AuthService();