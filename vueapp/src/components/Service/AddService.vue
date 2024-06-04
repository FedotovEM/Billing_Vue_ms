<template>
    <div class="container">
        <form @submit.prevent="addService">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="serviceName" class="form-label">Услуга</label>
                <input type="text" class="form-control" id="serviceName" aria-describedby="emailHelp" v-model="newServices.serviceName">
                <label for="unitsName" class="form-label">Единица измерения</label>
                <div>
                    <input list="unitsName" class="form-control" v-model="newServices.unitsName">
                    <datalist id="unitsName" style="width: 500px;">
                        <option v-for="item in unitList" style="width: 500px;">
                            {{ item.unitsName }}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/service">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newServices = reactive({
        serviceName: "",
        unitsName: ""
    });

    const router = useRouter();

    const unitList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Units", { headers: authHeader() })
            .then((response) => {
                unitList.value = response.data;
            })
    })
    const addService = async () => {
        axios.post(urls.nachServ + "/Services", newServices, { headers: authHeader() })
            .then(() => {
                router.push("/service");
            })
    }
</script>