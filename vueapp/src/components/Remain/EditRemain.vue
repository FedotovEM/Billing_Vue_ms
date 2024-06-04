<template>
    <div class="container">
        <form @submit.prevent="updateItem">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="editItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in accountList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="serviceName" class="form-control" v-model="editItem.serviceName">
                    <datalist id="serviceName">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>

                <label for="remmonth" class="form-label">Месяц расчета остатка</label>
                <input type="text" class="form-control" id="remmonth" aria-describedby="emailHelp" v-model="editItem.remmonth">

                <label for="remyear" class="form-label">Год расчета остатка</label>
                <input type="text" class="form-control" id="remyear" aria-describedby="emailHelp" v-model="editItem.remyear">

                <label for="remainsum" class="form-label">Сумма остатка</label>
                <input type="text" class="form-control" id="remainsum" aria-describedby="emailHelp" v-model="editItem.remainsum">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/remain">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editItem = reactive({
        remainCd: 0,
        accountCd: "",
        serviceName: "",
        remmonth: 0,
        remyear: 0,
        remainsum: 0,
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/Remains/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editItem.remainCd = route.params.id;
                editItem.accountCd = response.data.accountCd;
                editItem.serviceName = response.data.serviceName;
                editItem.remmonth = response.data.remmonth;
                editItem.remyear = response.data.remyear;
                editItem.remainsum = response.data.remainsum;
            })
    })

    const updateItem = async () => {
        axios.put(urls.webapi + `/Remains`, editItem, { headers: authHeader() })
            .then(() => {
                router.push("/remain");
            })
    }
    const accountList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                accountList.value = response.data;
            })
    })
    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
            })
    })
</script>