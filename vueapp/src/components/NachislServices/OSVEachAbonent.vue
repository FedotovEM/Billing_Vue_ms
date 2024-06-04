<template>
    <div>
        <form @submit.prevent="update">
            <div>
                <h4>Оборотно-сальдовая ведомость с расшифровкой по каждому абоненту</h4>

                <label for="accountCd" class="form-label">Услуга</label>
                <div>
                    <select class="form-control" v-model="payReq.serviceCd">
                        <option v-for="item in serviceList" :key="item.serviceCd" :value="item.serviceCd">
                            {{ item.serviceName + " (код: " + item.serviceCd + ")"}}
                        </option>
                    </select>
                    <h3>За период:</h3>
                    <div class="flex-container">
                        <h5 :style="{ marginLeft: '20px' }">c</h5>
                        <select v-model="payReq.startMonth" class="select-box">
                            <option v-for="(month, id) in months" :key="id" :value="month.id">
                                {{ month.name }}
                            </option>
                        </select>
                        <h5 :style="{ marginLeft: '20px' }">года</h5>
                        <select v-model="payReq.startYear" class="select-box">
                            <option v-for="(year, index) in years" :key="index">
                                {{ year }}
                            </option>
                        </select>

                        <h5 :style="{ marginLeft: '20px' }">по</h5>
                        <select v-model="payReq.endMonth" class="select-box">
                            <option v-for="(month, id) in months" :key="id" :value="month.id">
                                {{ month.name }}
                            </option>
                        </select>
                        <h5 :style="{ marginLeft: '20px' }">года</h5>
                        <select v-model="payReq.endYear" class="select-box">
                            <option v-for="(year, index) in years" :key="index">
                                {{ year }}
                            </option>
                        </select>
                    </div>

                    <button type="submit" class="btn btn-outline-primary">Составить ОСВ</button>
                </div>
                <div class="table-responsive" :style="{ marginTop: '30px' }">
                    <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                        <thead class="thead-light">
                            <tr>
                                <th rowspan="3" :style="{ textAlign: 'center' }">#</th>
                                <th rowspan="3" :style="{ textAlign: 'center' }">Л.Счет</th>
                                <th rowspan="3" :style="{ textAlign: 'center' }">Фамилия И.О.</th>
                                <th rowspan="3" :style="{ textAlign: 'center' }">Адрес</th>
                                <th colspan="2">Начало</th>
                                <th colspan="2">Оборот</th>
                                <th colspan="2">Конец</th>
                            </tr>
                            <tr>
                                <th rowspan="2">Дебет</th>
                                <th rowspan="2">Кредит</th>
                                <th colspan="1">Дебет</th>
                                <th colspan="1">Кредит</th>
                                <th rowspan="2">Дебет</th>
                                <th rowspan="2">Кредит</th>
                            </tr>
                            <tr>
                                <th class="col-sm-1" scope="col">Начислено</th>
                                <th class="col-sm-1" scope="col">Оплата</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, i) in OSVEachAbonentResponse.osvEachAbonent" :key="i">
                                <td> {{i + 1}} </td>
                                <td> {{item.accountcd}} </td>
                                <td> {{item.fio}} </td>
                                <td> {{item.address}} </td>
                                <td> {{item.beginDebetSum}} </td>
                                <td> {{item.beginKreditSum}} </td>
                                <td> {{item.nachislSum}} </td>
                                <td> {{item.paySum}} </td>
                                <td> {{item.finishDebetSum}} </td>
                                <td> {{item.finishKreditSum}} </td>
                            </tr>
                        </tbody>
                        <tfoot>
                            <tr>
                                <th colspan="4" :style="{ textAlign: 'right' }">Итого:</th>
                                <th>{{OSVEachAbonentResponse.totalStartDebet}}</th>
                                <th>{{OSVEachAbonentResponse.totalStartKredit}}</th>
                                <th>{{OSVEachAbonentResponse.totalNachisl}}</th>
                                <th>{{OSVEachAbonentResponse.totalPay}}</th>
                                <th>{{OSVEachAbonentResponse.totalFinishDebet}}</th>
                                <th>{{OSVEachAbonentResponse.totalFinishKredit}}</th>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            </div>
        </form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                selectedMonth: 1,
                months: [
                    { id: 1, name: 'Январь' },
                    { id: 2, name: 'Февраль' },
                    { id: 3, name: 'Март' },
                    { id: 4, name: 'Апрель' },
                    { id: 5, name: 'Май' },
                    { id: 6, name: 'Июнь' },
                    { id: 7, name: 'Июль' },
                    { id: 8, name: 'Август' },
                    { id: 9, name: 'Сентябрь' },
                    { id: 10, name: 'Октябрь' },
                    { id: 11, name: 'Ноябрь' },
                    { id: 12, name: 'Декабрь' },
                ],
                selectedYear: 2024,
                years: [
                    2018,
                    2019,
                    2020,
                    2021,
                    2022,
                    2023,
                    2024
                ]
            }
        }
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
        accountCd: "",
        startMonth: new Date().getMonth() + 1 - 1,
        endMonth: new Date().getMonth() + 1,
        startYear: new Date().getFullYear(),
        endYear: new Date().getFullYear(),
        serviceCd: 0,
    });

    const OSVEachAbonentResponse = ref([])    
    const update = async () => {
        axios.post(urls.nachServ + "/reportnachisls/osv-each-abonent", payReq, { headers: authHeader() })
            .then((response) => {
                OSVEachAbonentResponse.value = response.data;
            })
    }

    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
            })
    })

</script>

<style>
    .flex-container {
        display: flex;
        flex-wrap: wrap;
    }

    .select-box {
        width: 10%;
        margin-left: 20px;
    }

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