<template>
    <div class="container">
        <form @submit.prevent="addItem">
            <legend>Внесение нового платежа</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="newItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in abonentList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="service" class="form-control" v-model="newItem.serviceName">
                    <datalist id="service">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>
                <label for="receptionName" class="form-label">Пункт приема платежа</label>
                <div>
                    <input list="receptionName" class="form-control" v-model="newItem.receptionName">
                    <datalist id="receptionName">
                        <option v-for="item in receptionPointList">
                            {{ item.receptionName }}
                        </option>
                    </datalist>
                </div>

                <label for="payYear" class="form-label">Оплачиваемый год</label>
                <input type="text" class="form-control" id="payYear" aria-describedby="emailHelp" v-model="newItem.payYear">

                <label for="payMonth" class="form-label">Оплачиваемый месяц</label>
                <input type="text" class="form-control" id="payMonth" aria-describedby="emailHelp" v-model="newItem.payMonth">

                <label for="payType" class="form-label">Тип оплаты</label>
                <div id="v-model-select-dynamic" class="dropdown">
                    <select v-model="newItem.payType" class="form-control" data-live-search="true" aria-label="Default select example">
                        <option v-for="item in typeList">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <label for="payDate" class="form-label">Дата оплаты</label>
                <div class="form-group">
                    <input type="date" class="form-control" v-model="newItem.payDate">
                </div>

                <label for="paySum" class="form-label">Сумма оплаты</label>
                <input type="text" class="form-control" id="paySum" aria-describedby="emailHelp" v-model="newItem.paySum">
            </div>
            <button type="submit" class="btn btn-primary">Оплатить</button> |
            <RouterLink class="btn btn-primary" to="/paySumma">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newItem = reactive({
        accountCd: "",
        serviceName: "",
        receptionName: "",
        payYear: 0,
        payMonth: 0,
        payType: "",
        payDate: new Date().getFullYear() + "-" + (new Date().getMonth() + 1).toString().padStart(2, '0') + "-" + new Date().getDate().toString().padStart(2, '0'),
        paySum: 0,
    });

    const route = useRoute();
    const router = useRouter();
    const typeList = ["фактический", "нормативный"];

    onMounted(() => {
        axios.get(urls.payServ + `/pays/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                newItem.accountCd = response.data.accountCd;
                newItem.serviceName = response.data.serviceName;
                newItem.receptionName = response.data.receptionName;
                newItem.payYear = response.data.payYear;
                newItem.payMonth = response.data.payMonth;
                newItem.payType = response.data.payType;
                newItem.paySum = response.data.paySum;
            })

        axios.get(urls.payServ + `/PaySummas/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                newItem.accountCd = response.data.accountCd;
                newItem.serviceName = response.data.serviceName;
                newItem.receptionName = response.data.receptionName;
                newItem.payYear = response.data.payYear;
                newItem.payMonth = response.data.payMonth;
                newItem.payType = response.data.payType;
                newItem.paySum = response.data.paySum;
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
    const receptionPointList = ref([])
    onMounted(() => {
        axios.get(urls.payServ + "/ReceptionPoints", { headers: authHeader() })
            .then((response) => {
                receptionPointList.value = response.data;
            })
    })
    const addItem = async () => {
        axios.post(urls.payServ + "/PaySummas", newItem, { headers: authHeader() })
            .then(() => {
                router.push({ name: 'abonentPay', params: { id: newItem.accountCd } })
            })
    }
</script>