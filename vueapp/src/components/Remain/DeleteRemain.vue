<template>
    <div class="container">
        <form @submit.prevent="deletePrice">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет:</label>
                <h4 for="accountCd" class="form-label">{{deleteItem.accountCd}}</h4>

                <label for="serviceName" class="form-label">Услуга:</label>
                <h4 for="serviceName" class="form-label">{{deleteItem.serviceName}}</h4>

                <label for="remmonth" class="form-label">Месяц расчета остатка:</label>
                <h4 for="remmonth" class="form-label">{{deleteItem.remmonth}}</h4>

                <label for="remyear" class="form-label">Год расчета остатка:</label>
                <h4 for="remyear" class="form-label">{{deleteItem.remyear}}</h4>

                <label for="remainsum" class="form-label">Сумма остатка:</label>
                <h4 for="remainsum" class="form-label">{{deleteItem.remainsum}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/remain" type="button">Закрыть</RouterLink> |
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
        remainCd: 0,
        accountCd: "",
        serviceName: "",
        remmonth: 0,
        remyear: 0,
        remainsum: 0,
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.webapi + `/Remains/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteItem.remainCd = route.params.id;
                deleteItem.accountCd = response.data.accountCd;
                deleteItem.serviceName = response.data.serviceName;
                deleteItem.remmonth = response.data.remmonth;
                deleteItem.remyear = response.data.remyear;
                deleteItem.remainsum = response.data.remainsum;
            })
    })
    const deletePrice = async () => {
        axios.delete(urls.webapi + `/Remains/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/remain");
            })
    }
</script>