<template>
    <div class="container">
        <form @submit.prevent="update">
            <legend>Внесение показаний</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accounts" class="form-control" v-model="editNachislSumma.accountCd">
                    <datalist id="accounts">
                        <option v-for="item in abonentList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="service" class="form-control" v-model="editNachislSumma.serviceName">
                    <datalist id="service">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>

                <label for="nachislYear" class="form-label">Год начисления</label>
                <input type="text" class="form-control" id="nachislYear" aria-describedby="emailHelp" v-model="editNachislSumma.nachislYear">

                <label for="nachislMonth" class="form-label">Месяц начисления</label>
                <input type="text" class="form-control" id="nachislMonth" aria-describedby="emailHelp" v-model="editNachislSumma.nachislMonth">

                <label for="nachType" class="form-label">Тип начисления</label>
                <div id="v-model-select-dynamic" class="dropdown">
                    <select v-model="editNachislSumma.nachType" class="form-control" data-live-search="true" aria-label="Default select example">
                        <option v-for="item in typeList">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <label for="countResources" class="form-label">Количество ресурсов</label>
                <input type="text" class="form-control" id="countResources" aria-describedby="emailHelp" v-model="editNachislSumma.countResources">

                <label for="nachislSum" class="form-label">Сумма начисления</label>
                <input type="text" disabled="disabled" class="form-control" id="nachislSum" aria-describedby="emailHelp" v-model="editNachislSumma.nachislSum">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/nachislSumma">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editNachislSumma = reactive({
        nachislFactCd: 0,
        accountCd: "",
        serviceName: "",
        nachislSum: 0,
        nachislYear: 0,
        nachislMonth: 0,
        nachType: "",
        countResources: 0,
        abonentModeСd: 0,
        remainCd: -1
    });

    const route = useRoute();
    const router = useRouter();
    const typeList = ["фактический", "нормативный"];
    onMounted(() => {
        axios.get(urls.nachServ + `/nachisls/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editNachislSumma.remainCd = route.params.id;
                editNachislSumma.accountCd = response.data.accountCd;
                editNachislSumma.serviceName = response.data.serviceName;
                editNachislSumma.nachislSum = response.data.nachislSum;
                editNachislSumma.nachislYear = response.data.nachislYear;
                editNachislSumma.nachislMonth = response.data.nachislMonth;
                editNachislSumma.nachType = response.data.nachType;
                editNachislSumma.countResources = response.data.countResources;
                editNachislSumma.abonentModeСd = response.data.abonentModeСd;
            })

        axios.get(urls.nachServ + `/NachislSummas/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editNachislSumma.nachislFactCd = route.params.id;
                editNachislSumma.accountCd = response.data.accountCd;
                editNachislSumma.serviceName = response.data.serviceName;
                editNachislSumma.nachislSum = response.data.nachislSum;
                editNachislSumma.nachislYear = response.data.nachislYear;
                editNachislSumma.nachislMonth = response.data.nachislMonth;
                editNachislSumma.nachType = response.data.nachType;
                editNachislSumma.countResources = response.data.countResources;
                editNachislSumma.abonentModeСd = response.data.abonentModeСd;
            })
    })

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
    const update = async () => {
        axios.put(urls.nachServ + `/NachislSummas`, editNachislSumma, { headers: authHeader() })
            .then(() => {
                if (editNachislSumma.remainCd > 0) {
                    router.push("/paySumma/add/" + editNachislSumma.remainCd);
                }
                else {
                    router.push("/nachislSumma");
                }
            })
    }
</script>