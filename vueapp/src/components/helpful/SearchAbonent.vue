<template>
    <div class="container">
        <form @submit.prevent="postPayReq">
            <h1>Внести оплату за услугу</h1>
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
</template>

<script>
    import DropDown from './Dropdown';

    export default {
        name: 'nav-bar',
        props: ['menuItems'],
        components: {
            DropDown
        },
    }
</script>

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
    
    const postPayReq = async () => {
        axios.post(urls.payServ + "/pays/search-abonent", payReq, { headers: authHeader() })
            .then((response) => {
                abonentRemains.value = response.data;
            })
    }

</script>

<style>
    nav {
        display: flex;
        align-items: center;
    }
    nav .menu-item{
        color: #FFF;
        padding: 10px 20px;
        position: relative;
        text-align: center;
        border-bottom: 3px solid transparent;
        display: flex;
        transition: 0.4s;
    }

    nav .menu-item.active,
    nav .menu-item:hover{
        background-color: #444;
        border-bottom-color: #0026ff
    }

    nav .menu-item a{
        color: inherit;
        text-decoration: none;
    }
</style>