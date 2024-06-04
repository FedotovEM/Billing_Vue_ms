<template>
    <div class="container">
        <form @submit.prevent="deleteItem">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет:</label>
                <h4 for="accountCd" class="form-label">{{ItemToDelete.accountCd}}</h4>

                <label for="serviceName" class="form-label">Услуга:</label>
                <h4 for="serviceName" class="form-label">{{ItemToDelete.serviceName}}</h4>

                <label for="receptionName" class="form-label">Пункт приема платежа:</label>
                <h4 for="receptionName" class="form-label">{{ItemToDelete.receptionName}}</h4>

                <label for="payYear" class="form-label">Оплачиваемый год:</label>
                <h4 for="payYear" class="form-label">{{ItemToDelete.payYear}}</h4>

                <label for="payMonth" class="form-label">Оплачиваемый месяц:</label>
                <h4 for="payMonth" class="form-label">{{ItemToDelete.payMonth}}</h4>

                <label for="payType" class="form-label">Тип оплаты:</label>
                <h4 for="payType" class="form-label">{{ItemToDelete.payType}}</h4>

                <label for="payDate" class="form-label">Дата оплаты:</label>
                <h4 for="payDate" class="form-label">{{ItemToDelete.payDate.toString().replace('T00:00:00','')}}</h4>

                <label for="paySum" class="form-label">Сумма оплаты:</label>
                <h4 for="paySum" class="form-label">{{ItemToDelete.paySum}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/paySumma" type="button">Закрыть</RouterLink> |
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

    let ItemToDelete = reactive({
        payFactCd: 0,
        accountCd: "",
        serviceName: "",
        receptionName: "",
        payYear: 0,
        payMonth: 0,
        payType: "",
        payDate: new Date(),
        paySum: 0,
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.payServ + `/PaySummas/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                ItemToDelete.payFactCd = route.params.id;
                ItemToDelete.accountCd = response.data.accountCd;
                ItemToDelete.serviceName = response.data.serviceName;
                ItemToDelete.receptionName = response.data.receptionName;
                ItemToDelete.payYear = response.data.payYear;
                ItemToDelete.payMonth = response.data.payMonth;
                ItemToDelete.payType = response.data.payType;
                ItemToDelete.payDate = response.data.payDate;
                ItemToDelete.paySum = response.data.paySum;
            })
    })

    const deleteItem = async () => {
        axios.delete(urls.payServ + `/PaySummas/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/paySumma");
            })
    }
</script>