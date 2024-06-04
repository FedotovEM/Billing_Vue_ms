<template>
    <div class="container">
        <form @submit.prevent="addMode">
            <legend>Внесение значений начисления</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accounts" class="form-control" v-model="newNachislSumma.accountCd">
                    <datalist id="accounts">
                        <option v-for="item in abonentList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="service" class="form-control" v-model="newNachislSumma.serviceName">
                    <datalist id="service">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>
                <label for="nachislYear" class="form-label">Год начисления</label>
                <input type="text" class="form-control" id="nachislYear" aria-describedby="emailHelp" v-model="newNachislSumma.nachislYear">

                <label for="nachislMonth" class="form-label">Месяц начисления</label>
                <input type="text" class="form-control" id="nachislMonth" aria-describedby="emailHelp" v-model="newNachislSumma.nachislMonth">

                <label for="nachType" class="form-label">Тип начисления</label>
                <div id="v-model-select-dynamic" class="dropdown">
                    <select v-model="newNachislSumma.nachType" class="form-control" data-live-search="true" aria-label="Default select example">
                        <option v-for="item in typeList">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <label for="countResources" class="form-label">Количество ресурсов</label>
                <input type="text" class="form-control" id="countResources" aria-describedby="emailHelp" v-model="newNachislSumma.countResources">

                <label for="nachislSum" class="form-label">Сумма начисления</label>
                <input type="text" :disabled="newNachislSumma.countResources > 0" class="form-control" id="nachislSum" aria-describedby="emailHelp" v-model="newNachislSumma.nachislSum">
            </div>
            <button type="submit" class="btn btn-primary">Ввести</button> |
            <RouterLink class="btn btn-primary" to="/nachislSumma">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newNachislSumma = reactive({
        accountCd: "",
        serviceName: "",
        nachislSum: 0,
        nachislYear: 0,
        nachislMonth: 0,
        nachType: "",
        countResources: 0
    });

    const router = useRouter();
    const typeList = ["фактический", "нормативный"];

    const abonentList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                abonentList.value = response.data;
            })
    })
    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
            })
    })
    const addMode = async () => {
        axios.post(urls.nachServ + "/NachislSummas", newNachislSumma, { headers: authHeader() })
            .then(() => {
                router.push("/nachislSumma");
            })
    }
</script>