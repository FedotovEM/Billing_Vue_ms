<template>
    <div class="container">
        <form @submit.prevent="addExecutor">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="fio" class="form-label">Исполнитель</label>
                <input type="text" class="form-control" id="fio" aria-describedby="emailHelp" v-model="newExecutor.fio">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/executor">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newExecutor = reactive({
        fio: ""
    });

    const router = useRouter();

    const addExecutor = async () => {
        axios.post(urls.RepairReqServ + "/Executors", newExecutor, { headers: authHeader() })
            .then(() => {
                router.push("/executor");
            })
    }
</script>