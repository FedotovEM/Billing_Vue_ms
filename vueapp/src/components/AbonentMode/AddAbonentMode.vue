<template>
    <div class="container">
        <form @submit.prevent="addMode">
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

                <label for="modeName" class="form-label">Режим потребления</label>
                <div>
                    <input list="modeName" class="form-control" v-model="newItem.modeName" role="combobox" >
                    <datalist id="modeName" style="width: 500px;">
                        <option v-for="item in modeList">
                            {{ item.modeName + " [" + item.serviceName + "]"}}
                        </option>
                    </datalist>
                </div>

                <label for="counterr" class="form-label">Наличие счетчика:</label>
                <input type="checkbox" v-model="newItem.counterr">
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/abonentMode">Вернуться к списку</RouterLink>
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
        abonentModeCd: 0,
        accountCd: "",
        modeName: "",
        counterr: false,
    });

    const router = useRouter();

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
    const addMode = async () => {
        axios.post(urls.webapi + "/AbonentModes", newItem, { headers: authHeader() })
            .then(() => {
                router.push("/abonentMode");
            })
    }
</script>