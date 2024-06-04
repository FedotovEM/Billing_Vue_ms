<template>
    <div class="container">
        <form @submit.prevent="addUnit">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="unitsName" class="form-label">Единица измерения</label>
                <input type="text" class="form-control" id="unitsName" aria-describedby="emailHelp" v-model="newUnit.unitsName">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/unit">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newUnit = reactive({
        unitsName: ""
    });

    const router = useRouter();

    const addUnit = async () => {
        axios.post(urls.nachServ + "/Units", newUnit, { headers: authHeader() })
            .then(() => {
                router.push("/unit");
            })
    }
</script>