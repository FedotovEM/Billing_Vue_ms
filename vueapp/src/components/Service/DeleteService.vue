<template>
    <div class="container">
        <form @submit.prevent="deleteService">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="serviceName" class="form-label">Услуга:</label>
                <h4 for="serviceName" class="form-label">{{deleteServiceItem.serviceName}}</h4>
                <label for="unitsName" class="form-label">Единица измерения:</label>
                <h4 for="unitsName" class="form-label">{{deleteServiceItem.unitsName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/service" type="button">Закрыть</RouterLink> |
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

    let deleteServiceItem = reactive({
        serviceCd: 0,
        serviceName: "",
        unitsCd: 0,
        unitsName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Services/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteServiceItem.serviceCd = route.params.id;
                deleteServiceItem.serviceName = response.data.serviceName;
                deleteServiceItem.unitsCd = response.data.unitsCd;
                deleteServiceItem.unitsName = response.data.unitsName;
            })
    })

    const deleteService = async () => {
        axios.delete(urls.nachServ + `/Services/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/service");
            })
    }
</script>