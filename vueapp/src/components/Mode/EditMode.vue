<template>
    <div class="container">
        <form @submit.prevent="updateModes">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="modeName" class="form-label">Режим потребления</label>
                <input type="text" class="form-control" id="modeName" aria-describedby="emailHelp" v-model="editModes.modeName">
                <label for="norma" class="form-label">Норма</label>
                <input type="text" class="form-control" id="norma" aria-describedby="emailHelp" v-model="editModes.norma">
                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="serviceName" class="form-control" v-model="editModes.serviceName">
                    <datalist id="serviceName" style="width: 500px;">
                        <option v-for="item in serviceList" style="width: 500px;">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/mode">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editModes = reactive({
        modeCd: 0,
        modeName: "",
        norma: 0,
        serviceCd: 0,
        serviceName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Modes/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editModes.modeCd = route.params.id;
                editModes.norma = response.data.norma;
                editModes.modeName = response.data.modeName;
                editModes.serviceName = response.data.serviceName;
            })
    })

    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
            })
    })
    const updateModes = async () => {
        axios.put(urls.nachServ + `/Modes`, editModes, { headers: authHeader() })
            .then(() => {
                router.push("/mode");
            })
    }
</script>