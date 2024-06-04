<template>
    <div class="container">
        <form @submit.prevent="addItem">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="newItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in accountList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="serviceName" class="form-control" v-model="newItem.serviceName">
                    <datalist id="serviceName">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>

                <label for="remmonth" class="form-label">Месяц расчета остатка</label>
                <input type="text" class="form-control" id="remmonth" aria-describedby="emailHelp" v-model="newItem.remmonth">

                <label for="remyear" class="form-label">Год расчета остатка</label>
                <input type="text" class="form-control" id="remyear" aria-describedby="emailHelp" v-model="newItem.remyear">

                <label for="remainsum" class="form-label">Сумма остатка</label>
                <input type="text" class="form-control" id="remainsum" aria-describedby="emailHelp" v-model="newItem.remainsum">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/remain">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newItem = reactive({
        remainCd: 0,
        accountCd: "",
        serviceName: "",
        remmonth: 0,
        remyear: 0,
        remainsum: 0,
    });

    const router = useRouter();

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
    const addItem = async () => {
        axios.post(urls.webapi + "/Remains", newItem, { headers: authHeader() })
            .then(() => {
                router.push("/remain");
            })
    }
</script>