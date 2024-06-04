<template>
    <div class="container">
        <form @submit.prevent="deleteAbonent">
            <h1>Удаление!</h1>
            <h2>Вы уверены, что хотите удалить данного абонента?</h2>
            <div class="mb-3">
                <h4 for="accountCd" class="form-label">Лицевой счет: {{deleteItem.accountCd}}</h4>
                <h4 for="fio" class="form-label">ФИО: {{deleteItem.fio}}</h4>
                <h4 for="streetName" class="form-label">Улица: {{deleteItem.streetName}}</h4>
                <h4 for="houseNo" class="form-label">Номер дома: {{deleteItem.houseNo}}</h4>
                <h4 for="flatNo" class="form-label">Номер квартиры: {{deleteItem.flatNo}}</h4>
                <h4 for="phone" class="form-label">Телефон: {{deleteItem.phone}}</h4>
                <h4 for="corpus" class="form-label">Корпус: {{deleteItem.corpus}}</h4>
                <h4 for="district" class="form-label">Район: {{deleteItem.district}}</h4>
                <h4 for="countPerson" class="form-label">Количество зарегистрированных жителей: {{deleteItem.countPerson}}</h4>
                <h4 for="sizeFlat" class="form-label">Размер помещения (м2): {{deleteItem.sizeFlat}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/abonent" type="button">Закрыть</RouterLink> |
                <button type="submit" class="btn btn-danger">Подтвердить удаление</button>
            </div>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let deleteItem = reactive({
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
                deleteItem.accountCd = route.params.id;
                deleteItem.fio = response.data.fio;
                deleteItem.streetCd = response.data.streetCd;
                deleteItem.streetName = response.data.streetName;
                deleteItem.houseNo = response.data.houseNo;
                deleteItem.flatNo = response.data.flatNo;
                deleteItem.phone = response.data.phone;
                deleteItem.corpus = response.data.corpus;
                deleteItem.district = response.data.district;
                deleteItem.countPerson = response.data.countPerson;
                deleteItem.sizeFlat = response.data.sizeFlat;
            })
    })

    const deleteAbonent = async () => {
        axios.delete(urls.webapi + `/Abonents/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/abonent");
            })
    }
</script>