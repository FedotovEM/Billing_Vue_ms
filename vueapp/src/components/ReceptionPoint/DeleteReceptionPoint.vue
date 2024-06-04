<template>
    <div class="container">
        <form @submit.prevent="deleteReceptionPoint">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="receptionName" class="form-label">Пункт приема платежей:</label>
                <h4 for="receptionName" class="form-label">{{deleteReceptionPointItem.receptionName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/receptionPoint" type="button">Закрыть</RouterLink> |
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

    let deleteReceptionPointItem = reactive({
        receptionPointCd: 0,
        receptionName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.payServ + `/ReceptionPoints/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteReceptionPointItem.receptionName = response.data.receptionName;
                deleteReceptionPointItem.receptionPointCd = route.params.id;
            })
    })

    const deleteReceptionPoint = async () => {
        axios.delete(urls.payServ + `/ReceptionPoints/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/receptionPoint");
            })
    }
</script>