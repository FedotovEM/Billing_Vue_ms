<template>
    <div class="container">
        <form @submit.prevent="updateReceptionPoint">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="receptionName" class="form-label">Пункт приема платежей</label>
                <input type="text" class="form-control" id="receptionName" aria-describedby="emailHelp" v-model="editReceptionPoint.receptionName">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/receptionPoint">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editReceptionPoint = reactive({
        receptionPointCd: 0,
        receptionName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.payServ + `/ReceptionPoints/${route.params.id}`, { headers: authHeader() })
        .then((response) => {
            editReceptionPoint.receptionName = response.data.receptionName;
            editReceptionPoint.receptionPointCd = route.params.id;
        })
    })

    const updateReceptionPoint = async () => {
        axios.put(urls.payServ + `/ReceptionPoints`, editReceptionPoint, { headers: authHeader() })
            .then(() => {
                router.push("/receptionPoint");
            })
    }
</script>