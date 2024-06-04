<template>
    <div class="container">
        <form @submit.prevent="updateExecutor">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="fio" class="form-label">Исполнитель</label>
                <input type="text" class="form-control" id="fio" aria-describedby="emailHelp" v-model="editExecutors.fio">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/executor">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editExecutors = reactive({
        executorCd: 0,
        fio: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Executors/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editExecutors.fio = response.data.fio;
                editExecutors.executorCd = route.params.id;
            })
    })

    const updateExecutor = async () => {
        axios.put(urls.RepairReqServ + `/Executors`, editExecutors, { headers: authHeader() })
            .then(() => {
                router.push("/executor");
            })
    }
</script>