<template>
    <div class="container">
        <form @submit.prevent="addMode">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="modeName" class="form-label">Режим потребления</label>
                <input type="text" class="form-control" id="modeName" aria-describedby="emailHelp" v-model="newMode.modeName">
                <label for="norma" class="form-label">Норма</label>
                <input type="text" class="form-control" id="norma" aria-describedby="emailHelp" v-model="newMode.norma">
                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="serviceName" class="form-control" v-model="newMode.serviceName">
                    <datalist id="serviceName" style="width: 500px;">
                        <option v-for="item in serviceList" style="width: 500px;">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/mode">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newMode = reactive({
        modeCd: 0,
        modeName: "",
        norma: 0,
        serviceCd: 0,
        serviceName: ""
    });

    const router = useRouter();

    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
            })
    })
    const addMode = async () => {
        axios.post(urls.nachServ + "/Modes", newMode, { headers: authHeader() })
            .then(() => {
                router.push("/mode");
            })
    }
</script>