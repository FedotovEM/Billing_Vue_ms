<template>
    <div class="container">
        <form @submit.prevent="addDisrepair">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="failureName" class="form-label">Неисправность</label>
                <input type="text" class="form-control" id="failureName" aria-describedby="emailHelp" v-model="newDisrepair.failureName">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/disrepair">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newDisrepair = reactive({
        failureName: ""
    });

    const router = useRouter();

    const addDisrepair = async () => {
        axios.post(urls.RepairReqServ + "/Disrepairs", newDisrepair, { headers: authHeader() })
            .then(() => {
                router.push("/disrepair");
            })
    }
</script>