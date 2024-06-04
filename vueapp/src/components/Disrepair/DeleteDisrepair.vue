<template>
    <div class="container">
        <form @submit.prevent="deleteDisrepair">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="failureName" class="form-label">Неисправность:</label>
                <h4 for="failureName" class="form-label">{{deleteDisrepairItem.failureName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/disrepair" type="button">Закрыть</RouterLink> |
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

    let deleteDisrepairItem = reactive({
        failureCd: 0,
        failureName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Disrepairs/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteDisrepairItem.failureName = response.data.failureName;
                deleteDisrepairItem.failureCd = route.params.id;
            })
    })

    const deleteDisrepair = async () => {
        axios.delete(urls.RepairReqServ + `/Disrepairs/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/disrepair");
            })
    }
</script>