<template>
    <div class="container">
        <form @submit.prevent="deleteUnit">
            <h1>Удаление записи!</h1>
            <h3>Вы уверены, что хотите удалить данную запись?</h3>
            <div class="mb-3">
                <label for="fio" class="form-label">Единица измерения:</label>
                <h4 for="fio" class="form-label">{{deleteUnitItem.unitsName}}</h4>
            </div>
            <div>
                <RouterLink class="btn btn-primary" to="/unit" type="button">Закрыть</RouterLink> |
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

    let deleteUnitItem = reactive({
        unitCd: 0,
        unitsName: ""
    });
    const route = useRoute();
    const router = useRouter();

    onMounted(() => {
        axios.get(urls.nachServ + `/Units/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                deleteUnitItem.unitsName = response.data.unitsName;
                deleteUnitItem.unitCd = route.params.id;
            })
    })

    const deleteUnit = async () => {
        axios.delete(urls.nachServ + `/Units/${route.params.id}`, { headers: authHeader() })
            .then(() => {
                router.push("/unit");
            })
    }
</script>