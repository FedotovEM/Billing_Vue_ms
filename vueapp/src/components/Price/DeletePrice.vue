<template>
    <div class="container">
        <form @submit.prevent="deletePrice">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="serviceName" class="form-label">Режим потребления:</label>
                <h4 for="serviceName" class="form-label">{{deletePriceItem.modeName}}</h4>
                <label for="unitsName" class="form-label">Значение цены:</label>
                <h4 for="unitsName" class="form-label">{{deletePriceItem.priceValue}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/price" type="button">Закрыть</RouterLink> |
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

    let deletePriceItem = reactive({
        priceCd: 0,
        priceValue: "",
        modeName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Prices/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deletePriceItem.priceCd = route.params.id;
                deletePriceItem.priceValue = response.data.priceValue;
                deletePriceItem.modeName = response.data.modeName;
            })
    })

    const deletePrice = async () => {
        axios.delete(urls.nachServ + `/Prices/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/price");
            })
    }
</script>