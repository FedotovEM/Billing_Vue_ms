<template>
    <div class="container">
        <form @submit.prevent="updateUnit">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="unitsName" class="form-label">Единица измерения</label>
                <input type="text" class="form-control" id="unitsName" aria-describedby="emailHelp" v-model="editUnit.unitsName">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/unit">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editUnit = reactive({
        unitCd: 0,
        unitsName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Units/${route.params.id}`, { headers: authHeader() })
        .then((response) => {
            editUnit.unitsName = response.data.unitsName;
            editUnit.unitCd = route.params.id;
        })
    })

    const updateUnit = async () => {
        axios.put(urls.nachServ + `/Units`, editUnit, { headers: authHeader() })
            .then(() => {
                router.push("/unit");
            })
    }
</script>