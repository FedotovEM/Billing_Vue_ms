<template>
    <div class="container">
        <form @submit.prevent="addPrice">
            <legend>Добавление новой записи</legend>
            <div class="mb-3">
                <label for="priceValue" class="form-label">Значение цены</label>
                <input type="text" class="form-control" id="priceValue" aria-describedby="emailHelp" v-model="newPrices.priceValue">
                <label for="unitsName" class="form-label">Режим потребления</label>
                <div>
                    <input list="modeName" class="form-control" v-model="newPrices.modeName">
                    <datalist id="modeName" style="width: 500px;">
                        <option v-for="item in modeList" style="width: 500px;">
                            {{ item.modeName + " [" + item.serviceName + "]"}}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Добавить</button> |
            <RouterLink class="btn btn-primary" to="/price">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { reactive, ref, onMounted } from "vue";
    import { useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let newPrices = reactive({
        priceValue: "",
        modeName: ""
    });

    const router = useRouter();

    const modeList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Modes", { headers: authHeader() })
            .then((response) => {
                modeList.value = response.data;
            })
    })
    const addPrice = async () => {
        axios.post(urls.nachServ + "/Prices", newPrices, { headers: authHeader() })
            .then(() => {
                router.push("/price");
            })
    }
</script>