<template>
    <div class="container">
        <form @submit.prevent="deleteMode">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет:</label>
                <h4 for="accountCd" class="form-label">{{ItemToDelete.accountCd}}</h4>

                <label for="modeName" class="form-label">Режим потребления:</label>
                <h4 for="modeName" class="form-label">{{ItemToDelete.modeName}}</h4>

                <label for="counterr" class="form-label">Наличие счетчика:</label>
                <div v-if="ItemToDelete.counterr">
                    <input class="form-check-input" type="checkbox" disabled="disabled" checked>
                </div>
                <div v-if="!ItemToDelete.counterr">
                    <input class="form-check-input" type="checkbox" disabled="disabled">
                </div>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/abonentMode" type="button">Закрыть</RouterLink> |
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

    let ItemToDelete= reactive({
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
                ItemToDelete.abonentModeСd = route.params.id;
                ItemToDelete.accountCd = response.data.accountCd;
                ItemToDelete.modeName = response.data.modeName;
                ItemToDelete.counterr = response.data.counterr;
            })
    })

    const deleteMode = async () => {
        axios.delete(urls.webapi + `/AbonentModes/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/abonentMode");
            })
    }
</script>