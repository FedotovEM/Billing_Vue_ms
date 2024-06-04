<template>
    <div class="container">
        <form @submit.prevent="update">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="editItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in abonentList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="serviceName" class="form-label">Услуга</label>
                <div>
                    <input list="service" class="form-control" v-model="editItem.serviceName">
                    <datalist id="service">
                        <option v-for="item in serviceList">
                            {{ item.serviceName }}
                        </option>
                    </datalist>
                </div>
                <label for="receptionName" class="form-label">Пункт приема платежа</label>
                <div>
                    <input list="receptionName" class="form-control" v-model="editItem.receptionName">
                    <datalist id="receptionName">
                        <option v-for="item in receptionPointList">
                            {{ item.receptionName }}
                        </option>
                    </datalist>
                </div>

                <label for="payYear" class="form-label">Оплачиваемый год</label>
                <input type="text" class="form-control" id="payYear" aria-describedby="emailHelp" v-model="editItem.payYear">

                <label for="payMonth" class="form-label">Оплачиваемый месяц</label>
                <input type="text" class="form-control" id="payMonth" aria-describedby="emailHelp" v-model="editItem.payMonth">

                <label for="payType" class="form-label">Тип оплаты</label>
                <div id="v-model-select-dynamic" class="dropdown">
                    <select v-model="editItem.payType" class="form-control" data-live-search="true" aria-label="Default select example">
                        <option v-for="item in typeList">
                            {{ item }}
                        </option>
                    </select>
                </div>

                <label for="payDate" class="form-label">Дата оплаты</label>
                <div class="form-group">
                    <input type="date" class="form-control" v-model="editItem.payDate">
                </div>

                <label for="paySum" class="form-label">Сумма оплаты</label>
                <input type="text" class="form-control" id="paySum" aria-describedby="emailHelp" v-model="editItem.paySum">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/paySumma">Вернуться к списку</RouterLink>
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
        payFactCd: 0,
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
        axios.get(urls.payServ + `/PaySummas/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editItem.payFactCd = route.params.id;
                editItem.accountCd = response.data.accountCd;
                editItem.serviceName = response.data.serviceName;
                editItem.receptionName = response.data.receptionName;
                editItem.payYear = response.data.payYear;
                editItem.payMonth = response.data.payMonth;
                editItem.payType = response.data.payType;
                editItem.payDate = response.data.payDate.replace('T00:00:00', '');
                editItem.paySum = response.data.paySum;
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
    const update = async () => {
        axios.put(urls.payServ + `/PaySummas`, editItem, { headers: authHeader() })
            .then(() => {
                router.push("/paySumma");
            })
    }
</script>