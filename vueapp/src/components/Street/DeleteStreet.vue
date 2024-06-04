<template>
    <div class="container">
        <form @submit.prevent="deleteStreet">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="streetName" class="form-label">Наименование улицы:</label>
                <h4 for="streetName" class="form-label">{{deleteStreetItem.streetName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/street" type="button">Закрыть</RouterLink> |
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

    let deleteStreetItem = reactive({
        streetCd: 0,
        streetName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/Streets/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteStreetItem.streetName = response.data.streetName;
                deleteStreetItem.streetCd = route.params.id;
            })
    })

    const deleteStreet = async () => {
        axios.delete(urls.webapi + `/Streets/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/street");
            })
    }
</script>