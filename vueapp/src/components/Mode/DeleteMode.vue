<template>
    <div class="container">
        <form @submit.prevent="deleteMode">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="serviceName" class="form-label">Режим потребления:</label>
                <h4 for="serviceName" class="form-label">{{deleteModeItem.modeName}}</h4>
                <label for="unitsName" class="form-label">Норма:</label>
                <h4 for="unitsName" class="form-label">{{deleteModeItem.norma}}</h4>
                <label for="unitsName" class="form-label">Услуга:</label>
                <h4 for="unitsName" class="form-label">{{deleteModeItem.serviceName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/mode" type="button">Закрыть</RouterLink> |
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

    let deleteModeItem = reactive({
        modeCd: 0,
        modeName: "",
        norma: 0,
        serviceName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Modes/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteModeItem.modeCd = route.params.id;
                deleteModeItem.norma = response.data.norma;
                deleteModeItem.modeName = response.data.modeName;
                deleteModeItem.serviceName = response.data.serviceName;
            })
    })

    const deleteMode = async () => {
        axios.delete(urls.nachServ + `/Modes/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/mode");
            })
    }
</script>