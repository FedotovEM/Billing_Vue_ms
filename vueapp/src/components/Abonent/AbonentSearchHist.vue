<template>
    <div class="container">
        <form @submit.prevent="postPayReq">
            <h1>Просмотреть историю измений по абоненту</h1>
            <div class="mb-3">
                <h3>Найти абонента(-ов)</h3>
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <select class="form-control" v-model="payReq.accountCd">
                        <option v-for="item in abonentList" :key="item.accountCd">
                            {{ item.accountCd + " [" + item.fio + "]"}}
                        </option>
                    </select>
                </div>

                <label for="streetName" class="form-label">Улица</label>
                <div>
                    <select class="form-control" v-model="payReq.streetName">
                        <option v-for="item in streetList" :key="item.streetCd">
                            {{ item.streetName }}
                        </option>
                    </select>
                </div>

                <label for="houseNo" class="form-label">Номер дома</label>
                <input type="text" class="form-control" id="houseNo" aria-describedby="emailHelp" v-model="payReq.houseNo">

                <label for="flatNo" class="form-label">Номер квартиры</label>
                <input type="text" class="form-control" id="flatNo" aria-describedby="emailHelp" v-model="payReq.flatNo">

                <label for="corpus" class="form-label">Корпус</label>
                <input type="text" class="form-control" id="corpus" aria-describedby="emailHelp" v-model="payReq.corpus">
            </div>
            <button type="submit" class="btn btn-primary">Найти</button>
        </form>
    </div>
    <h1> </h1>
    <div class="paysD">
        <ul>
            <li v-for="abonent in abonentRemains" :key="abonent.accountCd">
                <div>
                    <h2></h2>
                    <h4>Абонент: л/с: {{abonent.accountCd}}, {{abonent.fio}}</h4>
                    <h4>Адрес: {{abonent.streetName}}, д.{{abonent.houseNo}}, кв.{{abonent.flatNo}}.</h4>
                    <h4>Кол-во зарегистрированных граждан: {{abonent.countPerson}}, размер помещения {{abonent.sizeFlat}}.</h4>

                    <div class="p-1">
                        <RouterLink class="btn btn-primary" :to="`/abonentHist/${abonent.accountCd}`">Просмотреть историю</RouterLink>
                    </div>
                </div>
            </li>
        </ul>
    </div>
    
</template>

<script setup>
    import { onMounted, reactive, ref } from "vue";
    import axios from 'axios';
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';
    
    let payReq = reactive({
        accountCd: "",
        streetName: "",
        houseNo: 0,
        flatNo: 0,
        corpus: 0
    });
    
    const abonentRemains = ref([])
    const abonentList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                abonentList.value = response.data;
            })
    })

    const streetList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Streets", { headers: authHeader() })
            .then((response) => {
                streetList.value = response.data;
            })
    })

    const postPayReq = async () => {
        axios.post(urls.webapi + "/Abonents/search-abonent", payReq, { headers: authHeader() })
            .then((response) => {
                abonentRemains.value = response.data;
            })
    }
    
</script>

<style>
    select.form-select {
        width: 400px;
    }    
    .form-control {
        position: relative;
        width: 400px;
    }
    .container {
        position: relative;
        top: 10px;
    }
    .paysD {
        position: absolute;
        margin-top: 20px;
    }
    th {
        cursor: pointer;
    }
</style>