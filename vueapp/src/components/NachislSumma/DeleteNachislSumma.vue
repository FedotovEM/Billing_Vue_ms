<template>
    <div class="container">
        <form @submit.prevent="deleteMode">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="accountCd" class="form-label">Лицевой счет:</label>
                <h4 for="accountCd" class="form-label">{{deleteNachislSummaItem.accountCd}}</h4>

                <label for="serviceName" class="form-label">Услуга:</label>
                <h4 for="serviceName" class="form-label">{{deleteNachislSummaItem.serviceName}}</h4>

                <label for="nachislSum" class="form-label">Сумма начисления:</label>
                <h4 for="nachislSum" class="form-label">{{deleteNachislSummaItem.nachislSum}}</h4>

                <label for="nachislYear" class="form-label">Год начисления:</label>
                <h4 for="nachislYear" class="form-label">{{deleteNachislSummaItem.nachislYear}}</h4>

                <label for="nachislMonth" class="form-label">Месяц начисления:</label>
                <h4 for="nachislMonth" class="form-label">{{deleteNachislSummaItem.nachislMonth}}</h4>

                <label for="nachType" class="form-label">Тип начисления:</label>
                <h4 for="nachType" class="form-label">{{deleteNachislSummaItem.nachType}}</h4>

                <label for="countResources" class="form-label">Количество ресурсов:</label>
                <h4 for="countResources" class="form-label">{{deleteNachislSummaItem.countResources}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/nachislSumma" type="button">Закрыть</RouterLink> |
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

    let deleteNachislSummaItem = reactive({
        nachislFactCd: 0,
        accountCd: "",
        serviceName: "",
        nachislSum: 0,
        nachislYear: 0,
        nachislMonth: 0,
        nachType: "",
        countResources: 0
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/NachislSummas/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteNachislSummaItem.nachislFactCd = route.params.id;
                deleteNachislSummaItem.accountCd = response.data.accountCd;
                deleteNachislSummaItem.serviceName = response.data.serviceName;
                deleteNachislSummaItem.nachislSum = response.data.nachislSum;
                deleteNachislSummaItem.nachislYear = response.data.nachislYear;
                deleteNachislSummaItem.nachislMonth = response.data.nachislMonth;
                deleteNachislSummaItem.nachType = response.data.nachType;
                deleteNachislSummaItem.countResources = response.data.countResources;
            })
    })

    const deleteMode = async () => {
        axios.delete(urls.nachServ + `/NachislSummas/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/nachislSumma");
            })
    }
</script>