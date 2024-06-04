<template>
    <div class="container">
        <form @submit.prevent="deleteExecutor">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="fio" class="form-label">Исполнитель:</label>
                <h4 for="fio" class="form-label">{{deleteExecutorItem.fio}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/executor" type="button">Закрыть</RouterLink> |
                <button type="submit" class="btn btn-danger">Подтвердить удаление</button>
            </div>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let deleteExecutorItem = reactive({
        executorCd: 0,
        fio: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Executors/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteExecutorItem.fio = response.data.fio;
                deleteExecutorItem.executorCd = route.params.id;
            })
    })

    const deleteExecutor = async () => {
        axios.delete(urls.RepairReqServ + `/Executors/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/executor");
            })
    }
</script>