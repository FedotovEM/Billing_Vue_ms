<template>
    <div class="container">
        <form @submit.prevent="addStreet">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="streetName" class="form-label">Наименование улицы</label>
                <input type="text" class="form-control" id="streetName" aria-describedby="emailHelp" v-model="newStreet.streetName">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/street">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newStreet = reactive({
        streetName: ""
    });

    const router = useRouter();

    const addStreet = async () => {
        axios.post(urls.webapi + "/Streets", newStreet, { headers: authHeader() })
            .then(() => {
                router.push("/street");
            })
    }
</script>