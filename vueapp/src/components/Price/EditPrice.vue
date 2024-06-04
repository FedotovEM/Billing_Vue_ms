<template>
    <div class="container">
        <form @submit.prevent="updatePrices">
            <legend>Обновление записи</legend>
            <div class="mb-3">
                <label for="priceValue" class="form-label">Значение цены</label>
                <input type="text" class="form-control" id="priceValue" aria-describedby="emailHelp" v-model="editPrices.priceValue">
                <label for="unitsName" class="form-label">Режим потребления</label>
                <div>
                    <input list="modeName" class="form-control" v-model="editPrices.modeName">
                    <datalist id="modeName" style="width: 500px;">
                        <option v-for="item in modeList" style="width: 500px;">
                            {{ item.modeName + " [" + item.serviceName + "]"}}
                        </option>
                    </datalist>
                </div>
            </div>
            <button type="submit" class="btn btn-primary">Обновить</button> |
            <RouterLink class="btn btn-primary" to="/price">Вернуться к списку</RouterLink>
        </form>
    </div>
</template>

<script setup>
    import axios from "axios";
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let editPrices = reactive({
        priceCd: 0,
        priceValue: "",
        modeName: ""
    });

    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Prices/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                editPrices.priceCd = route.params.id;
                editPrices.priceValue = response.data.priceValue;
                editPrices.modeName = response.data.modeName;
        })
    })

    const modeList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Modes", { headers: authHeader() })
            .then((response) => {
                modeList.value = response.data;
            })
    })
    const updatePrices = async () => {
        axios.put(urls.nachServ + `/Prices`, editPrices, { headers: authHeader() })
            .then(() => {
                router.push("/price");
            })
    }
</script>