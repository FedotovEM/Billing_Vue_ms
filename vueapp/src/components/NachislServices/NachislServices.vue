<template>
    <div>
        <div>
            <h4>Абонент: л/с: {{abonent.accountCd}}, {{abonent.fio}}</h4>
            <h4>Адрес: {{abonent.streetName}}, д.{{abonent.houseNo}}, кв.{{abonent.flatNo}}.</h4>
            <h4>Кол-во зарегистрированных граждан: {{abonent.countPerson}}, размер помещения {{abonent.sizeFlat}}.</h4>
            <div class="p-1">
                <RouterLink class="btn btn-primary" :to="`/abonent/edit/${abonent.accountCd}`">Редактировать</RouterLink>
            </div>
            <div class="table-responsive">
                <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                    <thead class="thead-light">
                        <tr>
                            <th scope="col" data-type="int">Код услуги</th>
                            <th class="col-sm-2" scope="col" data-type="string">Услуга</th>
                            <th class="col-sm-1" scope="col">Наличие счётчика</th>
                            <th class="col-sm-6" scope="col" data-type="int">Режим потребления</th>
                            <th class="col-sm-1" scope="col" data-type="int">Норматив потребления</th>
                            <th scope="col-sm-1" data-type="string">Тариф</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="item in abonent.abonentCards" :key="item.serviceCd">
                            <td> {{item.serviceCd}} </td>
                            <td> {{item.serviceName + " (" + item.unitsName + ")"}} </td>
                            <td v-if="item.counterr">
                                <input type="checkbox" disabled="disabled" checked>
                            </td>
                            <td v-if="!item.counterr">
                                <input type="checkbox" disabled="disabled">
                            </td>
                            <td> {{item.modeName}} </td>
                            <td> {{item.norma + " (" + item.normaName + ")"}} </td>
                            <td> {{item.priceValue + " (" + item.priceName + ")"}} </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: ['accountCd']
    }
</script>

<script setup>
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import axios from 'axios';
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    const route = useRoute();
    let payReq = reactive({
        accountCd: route.params.id,
        streetName: "",
        houseNo: 0,
        flatNo: 0,
        corpus: 0
    });

    const abonent = ref([])    
    onMounted(() => {
        axios.post(urls.nachServ + "/nachisls/search-abonent", payReq, { headers: authHeader() })
            .then((response) => {
                abonent.value = response.data;
            })
    })

</script>

<style>
    select.form-select {
        width: 400px;
    }

    .form-control {
        position: relative;
        width: 400px;
    }

    th {
        cursor: pointer;
    }
</style>