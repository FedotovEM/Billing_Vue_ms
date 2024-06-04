<template>
    <div class="container">
        <form @submit.prevent="update">
            <legend>Обновление данных по абоненту</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <input type="text" class="form-control" id="accountCd" aria-describedby="emailHelp" v-model="editItem.accountCd">

                <label for="fio" class="form-label">ФИО</label>
                <input type="text" class="form-control" id="fio" aria-describedby="emailHelp" v-model="editItem.fio">

                <label for="streetName" class="form-label">Улица</label>
                <div>
                    <input list="street" class="form-control" v-model="editItem.streetName">
                    <datalist id="street">
                        <option v-for="item in streetList">
                            {{ item.streetName }}
                        </option>
                    </datalist>
                </div>

                <label for="houseNo" class="form-label">Номер дома</label>
                <input type="text" class="form-control" id="houseNo" aria-describedby="emailHelp" v-model="editItem.houseNo">

                <label for="flatNo" class="form-label">Номер квартиры</label>
                <input type="text" class="form-control" id="flatNo" aria-describedby="emailHelp" v-model="editItem.flatNo">

                <label for="phone" class="form-label">Телефон</label>
                <input type="text" class="form-control" id="phone" aria-describedby="emailHelp" v-model="editItem.phone">

                <label for="corpus" class="form-label">Корпус</label>
                <input type="text" class="form-control" id="corpus" aria-describedby="emailHelp" v-model="editItem.corpus">

                <label for="district" class="form-label">Район</label>
                <input type="text" class="form-control" id="district" aria-describedby="emailHelp" v-model="editItem.district">

                <label for="countPerson" class="form-label">Количество зарегистрированных жителей</label>
                <input type="text" class="form-control" id="countPerson" aria-describedby="emailHelp" v-model="editItem.countPerson">

                <label for="sizeFlat" class="form-label">Размер помещения (м2)</label>
                <input type="text" class="form-control" id="sizeFlat" aria-describedby="emailHelp" v-model="editItem.sizeFlat">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/abonent">Вернуться к списку</RouterLink>
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
        accountCd: "",
        fio: "",
        streetCd: 0,
        streetName: "",
        houseNo: 0,
        flatNo: 0,
        phone: "",
        corpus: 0,
        district: "",
        countPerson: 0,
        sizeFlat: 0,
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/Abonents/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editItem.accountCd = route.params.id;
                editItem.fio = response.data.fio;
                editItem.streetCd = response.data.streetCd;
                editItem.streetName = response.data.streetName;
                editItem.houseNo = response.data.houseNo;
                editItem.flatNo = response.data.flatNo;
                editItem.phone = response.data.phone;
                editItem.corpus = response.data.corpus;
                editItem.district = response.data.district;
                editItem.countPerson = response.data.countPerson;
                editItem.sizeFlat = response.data.sizeFlat;
            })
    })

    const streetList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Streets", { headers: authHeader() })
            .then((response) => {
                streetList.value = response.data;
            })
    })
    const update = async () => {
        axios.put(urls.webapi + `/Abonents`, editItem, { headers: authHeader() })
            .then(() => {
                router.push("/abonent");
            })
    }
</script>