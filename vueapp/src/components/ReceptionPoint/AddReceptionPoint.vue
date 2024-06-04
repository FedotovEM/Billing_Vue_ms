<template>
    <div class="container">
        <form @submit.prevent="addReceptionPoint">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="receptionName" class="form-label">Пункт приема платежей</label>
                <input type="text" class="form-control" id="receptionName" aria-describedby="emailHelp" v-model="newReceptionPoint.receptionName">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/receptionPoint">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newReceptionPoint = reactive({
        receptionName: ""
    });

    const router = useRouter();

    const addReceptionPoint = async () => {
        axios.post(urls.payServ + "/ReceptionPoints", newReceptionPoint, { headers: authHeader() })
            .then(() => {
                router.push("/receptionPoint");
            })
    }
</script>