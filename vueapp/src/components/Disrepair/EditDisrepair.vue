<template>
    <div class="container">
        <form @submit.prevent="updateDisrepair">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="failureName" class="form-label">Неисправность</label>
                <input type="text" class="form-control" id="failureName" aria-describedby="emailHelp" v-model="editDisrepair.failureName">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/disrepair">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editDisrepair = reactive({
        failureCd: 0,
        failureName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Disrepairs/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editDisrepair.failureName = response.data.failureName;
                editDisrepair.failureCd = route.params.id;
            })
    })

    const updateDisrepair = async () => {
        axios.put(urls.RepairReqServ + `/Disrepairs`, editDisrepair, { headers: authHeader() })
            .then(() => {
                router.push("/disrepair");
            })
    }
</script>