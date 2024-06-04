<template>
    <div class="container">
        <form @submit.prevent="updateStreet">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="streetName" class="form-label">Наименование улицы</label>
                <input type="text" class="form-control" id="streetName" aria-describedby="emailHelp" v-model="editStreet.streetName">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/street">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editStreet = reactive({
        streetCd: 0,
        streetName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/Streets/${route.params.id}`, { headers: authHeader() })
        .then((response) => {
            editStreet.streetName = response.data.streetName;
            editStreet.streetCd = route.params.id;
        })
    })

    const updateStreet = async () => {
        axios.put(urls.webapi + `/Streets`, editStreet, { headers: authHeader() })
            .then(() => {
                router.push("/street");
            })
    }
</script>