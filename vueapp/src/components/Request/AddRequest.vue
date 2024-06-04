<template>
    <div class="container">
        <form @submit.prevent="addItem">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет</label>
                <div>
                    <input list="accountCd" class="form-control" v-model="newItem.accountCd">
                    <datalist id="accountCd">
                        <option v-for="item in accountList">
                            {{ item.accountCd }}
                        </option>
                    </datalist>
                </div>

                <label for="failureName" class="form-label">Неисправность</label>
                <div>
                    <input list="failureName" class="form-control" v-model="newItem.failureName">
                    <datalist id="failureName">
                        <option v-for="item in failuretList">
                            {{ item.failureName }}
                        </option>
                    </datalist>
                </div>

                <label for="fio" class="form-label">ФИО исполнителя</label>
                <div>
                    <input list="fio" class="form-control" v-model="newItem.fio">
                    <datalist id="fio">
                        <option v-for="item in executorList">
                            {{ item.fio }}
                        </option>
                    </datalist>
                </div>

                <label for="incomingDate" class="form-label">Поступление заявки</label>
                <div class="form-group">
                    <input type="date" class="form-control" v-model="newItem.incomingDate">
                </div>

                <label for="executionDate" class="form-label">Выполнение заявки</label>
                <div class="form-group">
                    <input type="date" class="form-control" v-model="newItem.executionDate">
                </div>

                <label for="executed" class="form-label">Погашена: </label>
                <input type="checkbox" v-model="newItem.executed">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/request">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newItem = reactive({
        requestCd: 0,
        accountCd: "",
        failureName: "",
        fio: "",
        incomingDate: new Date().getFullYear() + "-" + (new Date().getMonth() + 1).toString().padStart(2, '0') + "-" + new Date().getDate().toString().padStart(2, '0'),
        executionDate: null,
        executed: false,
    });

    const router = useRouter();

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
    const addItem = async () => {
        axios.post(urls.RepairReqServ + "/Requests", newItem, { headers: authHeader() })
            .then(() => {
                router.push("/request");
            })
    }
</script>