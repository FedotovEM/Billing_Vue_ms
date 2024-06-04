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

                <label for="modeName" class="form-label">Режим потребления</label>
                <div>
                    <input list="modeName" class="form-control" v-model="editItem.modeName">
                    <datalist id="modeName">
                        <option v-for="item in modeList">
                            {{ item.modeName + " [" + item.serviceName + "]"}}
                        </option>
                    </datalist>
                </div>

                <label for="counterr" class="form-label">Наличие счетчика:</label>
                <input type="checkbox" v-model="editItem.counterr">
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/abonentMode">Вернуться к списку</RouterLink>
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
        abonentModeСd: 0,
        accountCd: "",
        modeName: "",
        counterr: false,
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/AbonentModes/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editItem.abonentModeСd = route.params.id;
                editItem.accountCd = response.data.accountCd;
                editItem.modeName = response.data.modeName;
                editItem.counterr = response.data.counterr;
            })
    })

    const accountList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                accountList.value = response.data;
            })
    })
    const modeList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Modes", { headers: authHeader() })
            .then((response) => {
                modeList.value = response.data;
            })
    })
    const updateItem = async () => {
        axios.put(urls.webapi + `/AbonentModes`, editItem, { headers: authHeader() })
            .then(() => {
                router.push("/abonentMode");
            })
    }
</script>