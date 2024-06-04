<template>
    <div class="container">
        <form @submit.prevent="addItem">
            <legend>Добавление нового абонента</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <input type="text" class="form-control" id="accountCd" aria-describedby="emailHelp" v-model="newItem.accountCd">

                <label for="fio" class="form-label">ФИО</label>
                <input type="text" class="form-control" id="fio" aria-describedby="emailHelp" v-model="newItem.fio">

                <label for="streetName" class="form-label">Улица</label>
                <div>
                    <select class="form-control" v-model="newItem.streetName">
                        <option v-for="item in streetList" :key="item.streetCd" :value="item.streetName">
                            {{ item.streetName }}
                        </option>
                    </select>
                </div>


                <label for="houseNo" class="form-label">Номер дома</label>
                <input type="text" class="form-control" id="houseNo" aria-describedby="emailHelp" v-model="newItem.houseNo">

                <label for="flatNo" class="form-label">Номер квартиры</label>
                <input type="text" class="form-control" id="flatNo" aria-describedby="emailHelp" v-model="newItem.flatNo">

                <label for="phone" class="form-label">Телефон</label>
                <input type="text" class="form-control" id="phone" aria-describedby="emailHelp" v-model="newItem.phone">

                <label for="corpus" class="form-label">Корпус</label>
                <input type="text" class="form-control" id="corpus" aria-describedby="emailHelp" v-model="newItem.corpus">

                <label for="district" class="form-label">Район</label>
                <input type="text" class="form-control" id="district" aria-describedby="emailHelp" v-model="newItem.district">

                <label for="countPerson" class="form-label">Количество зарегистрированных жителей</label>
                <input type="text" class="form-control" id="countPerson" aria-describedby="emailHelp" v-model="newItem.countPerson">

                <label for="sizeFlat" class="form-label">Размер помещения (м2)</label>
                <input type="text" class="form-control" id="sizeFlat" aria-describedby="emailHelp" v-model="newItem.sizeFlat">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/abonent">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<style>
    input.form-control-sm {
        position: relative;
        display: inline-block;
        width: 300px;
        left: 20px;
    }

    select.form-select {
        position: relative;
        width: 400px;
    }   
</style>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newItem = reactive({
        accountCd: "",
        fio: "",
        streetName: "",
        houseNo: 0,
        flatNo: 0,
        phone: "",
        corpus: 0,
        district: "",
        countPerson: 0,
        sizeFlat: 0,
    });

    const router = useRouter();

    const streetList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Streets", { headers: authHeader() })
            .then((response) => {
                streetList.value = response.data;
            })
    })
    const addItem = async () => {
        axios.post(urls.webapi + "/Abonents", newItem, { headers: authHeader() })
            .then(() => {
                router.push("/abonent");
            })
    }
</script>