<template>
    <div class="container">
        <link rel="stylesheet" href="https://unpkg.com/@jarstone/dselect/dist/css/dselect.css">
        
        <form @submit.prevent="GetStatistics">
            <div class="d-flex align-items-center justify-content-center p-2">
                <RouterLink class="btn btn-outline-primary translate-middle mr-2" to="/abonent">Количество абонентов в системе: {{newItem.countAbonent}}</RouterLink>
                <RouterLink class="btn btn-outline-primary translate-middle mr-2" to="/paySumma">Количество платежей в системе: {{newItem.countPays}}</RouterLink>
                <RouterLink class="btn btn-outline-primary translate-middle mr-2" to="/nachislSumma">Количество начислений в системе: {{newItem.countNachisl}}</RouterLink>
            </div>
            <div class="d-flex align-items-center justify-content-center p-2">
                <RouterLink class="btn btn-outline-primary mr-2" to="/service">Количество услуг в системе: {{newItem.countServices}}</RouterLink>
                <RouterLink class="btn btn-outline-primary mr-2" to="/request">Количество заявок на ремонт в системе: {{newItem.countRequests}}</RouterLink>
                <RouterLink class="btn btn-outline-primary mr-2" to="/executor">Количество исполнителей в системе: {{newItem.countExecutors}}</RouterLink>
            </div>
            <div class="d-flex align-items-center justify-content-center p-2">
                <RouterLink class="btn btn-outline-primary mr-2" to="/street">Количество улиц в системе: {{newItem.countStreets}}</RouterLink>
            </div>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import authHeader from '../services/auth-header.js';
    import { urls } from '../settings.js';

    let newItem = reactive({
        countAbonent: 0,
        countPays: 0,
        countNachisl: 0,
        countServices: 0,
        countRequests: 0,
        countExecutors: 0,
        countStreets: 0,
    });
    

    onMounted(() => {
        axios.get(urls.webapi + "/Statistics/get-abonent-stat", { headers: authHeader() })
            .then((response) => {
                newItem.countAbonent = response.data.countAbonent;;
                newItem.countStreets = response.data.countStreets;
            });
        axios.post(urls.payServ + "/get-stat-count", { headers: authHeader() })
            .then((response) => {
                newItem.countPays = response.data;
            })
    });
</script>
