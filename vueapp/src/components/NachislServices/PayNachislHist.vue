<template>
    <div>
        <form @submit.prevent="update">
            <div>
                <h4>Абонент: л/с: {{abonent.accountCd}}, {{abonent.fio}}</h4>
                <h4>Адрес: {{abonent.streetName}}, д.{{abonent.houseNo}}, кв.{{abonent.flatNo}}.</h4>
                <h4>Кол-во зарегистрированных граждан: {{abonent.countPerson}}, размер помещения {{abonent.sizeFlat}}.</h4>
                <label for="accountCd" class="form-label">Услуга</label>
                <div>
                    <select class="form-control" v-model="payReq.serviceCd">
                        <option v-for="item in serviceList" :key="item.serviceCd" :value="item.serviceCd">
                            {{ item.serviceName + " (код: " + item.serviceCd + ")"}}
                        </option>
                    </select>
                    <button type="submit" class="btn btn-outline-primary">Вывести историю начислений и платежей</button>
                </div>
                <div class="table-responsive" :style="{ marginTop: '30px' }">
                    <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                        <thead class="thead-light">
                            <tr>
                                <th class="col-sm-1" scope="col">#</th>
                                <th class="col-sm-3" scope="col">Услуга</th>
                                <th class="col-sm-2" scope="col">Учетный месяц</th>
                                <th class="col-sm-1" scope="col">Начало</th>
                                <th class="col-sm-1" scope="col">Начислено</th>
                                <th class="col-sm-1" scope="col">Факт. объем</th>
                                <th class="col-sm-1" scope="col">Оплата</th>
                                <th class="col-sm-1" scope="col">Конец</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, i) in payNachislHist" :key="i">
                                <td> {{i + 1}} </td>
                                <td> {{item.serviceName}} </td>
                                <td> {{item.accountingMonth}} </td>
                                <td> {{item.beginRemainSum}} </td>
                                <td> {{item.nachislSum}} </td>
                                <td> {{item.countResources}} </td>
                                <td> {{item.paySum}} </td>
                                <td> {{item.endRemainSum}} </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </form>
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
    
    let service = "";
    const route = useRoute();
    let payReq = reactive({
        accountCd: route.params.id,
        serviceCd: 0,
    });

    const payNachislHist = ref([])
    
    const update = async () => {
        axios.post(urls.nachServ + "/nachisls/pay-nachisl-hist/", payReq, { headers: authHeader() })
            .then((response) => {
                payNachislHist.value = response.data;
            })
    }

    const abonent = ref([])
    onMounted(() => {
        axios.get(urls.webapi + `/Abonents/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                 abonent.value = response.data;
            })
    })

    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
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