<template>
    <div class="container">
        <form @submit.prevent="deleteItem">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет:</label>
                <h4 for="accountCd" class="form-label">{{itemToDelete.accountCd}}</h4>

                <label for="failureName" class="form-label">Неисправность:</label>
                <h4 for="failureName" class="form-label">{{itemToDelete.failureName}}</h4>

                <label for="fio" class="form-label">ФИО исполнителя:</label>
                <h4 for="fio" class="form-label">{{itemToDelete.fio}}</h4>

                <label for="incomingDate" class="form-label">Поступление заявки:</label>
                <h4 for="incomingDate" class="form-label">{{itemToDelete.incomingDate.toString().replace('T00:00:00','')}}</h4>

                <label for="executionDate" class="form-label">Выполнение заявки:</label>
                <div v-if="itemToDelete.executionDate">
                    <h4 for="executionDate" class="form-label">{{itemToDelete.executionDate.replace('T00:00:00','')}}</h4>
                </div>
                <div v-if="!itemToDelete.executionDate"></div>

                <label for="executed" class="form-label">Погашена:</label>
                <div v-if="itemToDelete.executed">
                    <input class="form-check-input" type="checkbox" disabled="disabled" checked>
                </div>
                <div v-if="!itemToDelete.executed">
                    <input class="form-check-input" type="checkbox" disabled="disabled">
                </div>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/request" type="button">Закрыть</RouterLink> |
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

    let itemToDelete = reactive({
        requestCd: 0,
        accountCd: "",
        failureName: "",
        fio: "",
        incomingDate: new Date(),
        executionDate: null,
        executed: false,
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Requests/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                itemToDelete.requestCd = route.params.id;
                itemToDelete.accountCd = response.data.accountCd;
                itemToDelete.failureName = response.data.failureName;
                itemToDelete.fio = response.data.fio;
                itemToDelete.incomingDate = response.data.incomingDate;
                itemToDelete.executionDate = response.data.executionDate;
                itemToDelete.executed = response.data.executed;
            })
    })

    const deleteItem = async () => {
        axios.delete(urls.RepairReqServ + `/Requests/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/request");
            })
    }
</script>