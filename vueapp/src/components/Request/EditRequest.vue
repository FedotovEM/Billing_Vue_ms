<template>
    <div class="container">
        <form @submit.prevent="updateItem">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="editItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in accountList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="failureName" class="form-label">Неисправность</label>
                <div>
                    <input list="failureName" class="form-control" v-model="editItem.failureName">
                    <datalist id="failureName">
                        <option v-for="item in failuretList">
                            {{ item.failureName }}
                        </option>
                    </datalist>
                </div>

                <label for="fio" class="form-label">ФИО исполнителя</label>
                <div>
                    <input list="fio" class="form-control" v-model="editItem.fio">
                    <datalist id="fio">
                        <option v-for="item in executorList">
                            {{ item.fio }}
                        </option>
                    </datalist>
                </div>

                <label for="incomingDate" class="form-label">Поступление заявки</label>
                <div class="form-group" id="incomingDate">
                    <input type="date" class="form-control" v-model="editItem.incomingDate">
                </div>

                <label for="executionDate" class="form-label">Выполнение заявки</label>
                <div class="form-group" id="incomingDate">
                    <input type="date" class="form-control" v-model="editItem.executionDate">
                </div>

                <label for="executed" class="form-label">Погашена: </label>
                <input type="checkbox" v-model="editItem.executed">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/request">Вернуться к списку</RouterLink>
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
        requestCd: 0,
        accountCd: "",
        failureName: "",
        fio: "",
        incomingDate: new Date().getFullYear() + "-" + (new Date().getMonth() + 1).toString().padStart(2, '0') + "-" + new Date().getDate().toString().padStart(2, '0'),
        executionDate: null,
        executed: false,
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.RepairReqServ + `/Requests/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editItem.requestCd = route.params.id;
                editItem.accountCd = response.data.accountCd;
                editItem.failureName = response.data.failureName;
                editItem.fio = response.data.fio;
                editItem.incomingDate = response.data.incomingDate.replace('T00:00:00', '');
                editItem.executionDate = response.data.executionDate.replace('T00:00:00', '');
                editItem.executed = response.data.executed;
        })
    })
    const updateItem = async () => {
        axios.put(urls.RepairReqServ + `/Requests`, editItem, { headers: authHeader() })
            .then(() => {
                router.push("/request");
            })
    }

    const accountList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                accountList.value = response.data;
            })
    })
    const failuretList = ref([])
    onMounted(() => {
        axios.get(urls.RepairReqServ + "/Disrepairs", { headers: authHeader() })
            .then((response) => {
                failuretList.value = response.data;
            })
    })
    const executorList = ref([])
    onMounted(() => {
        axios.get(urls.RepairReqServ + "/Executors", { headers: authHeader() })
            .then((response) => {
                executorList.value = response.data;
            })
    })

</script>