<template>
    <div class="container">
        <form @submit.prevent="updateServices">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="serviceName" class="form-label">Услуга</label>
                <input type="text" class="form-control" id="serviceName" aria-describedby="emailHelp" v-model="editServices.serviceName">
                <label for="unitsName" class="form-label">Единица измерения</label>
                <div>
                    <input list="unitsName" class="form-control" v-model="editServices.unitsName">
                    <datalist id="unitsName" style="width: 500px;">
                        <option v-for="item in unitList" style="width: 500px;">
                            {{ item.unitsName }}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/service">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive, ref } from "vue";
import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editServices = reactive({
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
                editServices.serviceCd = route.params.id;
                editServices.serviceName = response.data.serviceName;
                editServices.unitsCd = response.data.unitsCd;
                editServices.unitsName = response.data.unitsName;
        })
    })

    const unitList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Units", { headers: authHeader() })
            .then((response) => {
                unitList.value = response.data;
            })
    })
    const updateServices = async () => {
        axios.put(urls.nachServ + `/Services`, editServices, { headers: authHeader() })
            .then(() => {
                router.push("/service");
            })
    }
</script>