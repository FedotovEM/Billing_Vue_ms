<template>
    <div class="container">
        <form @submit.prevent="updateItem">
            <h1 class="text-center">Отчет о ремонтных заявках за месяц</h1>
            <h6>Составить отчет за месяц</h6>
            <div class="grid-container">
                <select v-model="searchRep.month" class="form-control">
                    <option v-for="(month, id) in months" :key="id" :value="month.id">
                        {{ month.name }}
                    </option>
                </select>
                <h6>года</h6>
                <select v-model="searchRep.year" class="form-control">
                    <option v-for="(year, index) in years" :key="index">
                        {{ year }}
                    </option>
                </select>
            </div>
            <button type="submit" class="btn btn-outline-primary">Составить отчёт</button>

            <ul>
                <div v-if="isReq == true && ReportByMonth.sumCountRequest == 0">
                    <h6>В данном месяце заявки на ремонт не подавались</h6>
                </div>

                <li v-for="(failure, i) in ReportByMonth.reportResponseList" :key="i">
                    <h6>Неисправность:</h6>
                    <h6>{{failure.failureName}}</h6>
                    <div class="row">
                        <div class="col-md-7">
                            <h6 style="margin-left: 360px;">За пред. мес.: {{failure.fullCountPreviousMonth}}</h6>
                        </div>
                        <div class="col-md-5">
                            <h6 style="margin-left: 75px;">За данный мес.: {{failure.fullCountRequest}}</h6>
                        </div>
                    </div>

                    <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                        <thead class="thead-light">
                            <tr>
                                <th style="width: 200px;">Исполнитель</th>
                                <th style="width: 200px;">За пред. мес.</th>
                                <th style="width: 200px;">За данный мес.</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(executor, index) in failure.executorsToReportMonths" :key="index">
                                <td>{{ executor.executorFIO }}</td>
                                <td>{{ executor.countPreviousMonth }}</td>
                                <td>{{ executor.countRequest }}</td>
                            </tr>
                        </tbody>
                    </table>
                </li>

                <table class="table table-hover table-bordered" style="width:100%" v-if="ReportByMonth.sumCountRequest">
                    <thead class="thead-light">
                        <tr>
                            <th style="width: 200px;">Всего:</th>
                            <th style="width: 200px;">За пред. мес.</th>
                            <th style="width: 200px;">За данный мес.</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(failure, index) in ReportByMonth.reportResponseList" :key="index">
                            <td>{{ failure.failureName }}</td>
                            <td>{{ failure.fullCountPreviousMonth }}</td>
                            <td>{{ failure.fullCountRequest }}</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <th>Итого:</th>
                            <th>{{ ReportByMonth.sumCountPreviousMonth }}</th>
                            <th>{{ ReportByMonth.sumCountRequest }}</th>
                        </tr>
                    </tfoot>
                </table>
            </ul>
            
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
    import { onMounted, ref, reactive } from "vue";
    import axios from 'axios';
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    let isReq = false;
    let searchRep = reactive({
        month: 1,
        year: 2024
    });

    const ReportByMonth = ref([]);
    const updateItem = async () => {
        axios.post(urls.RepairReqServ + "/reportreq/request-report-month", searchRep, { headers: authHeader() })
            .then((response) => {
                ReportByMonth.value = response.data;
                isReq = true;
            })
    }
</script>

<style>
    .btn-outline-primary {
        position: relative;
        left: 6px;
        top: 8px;
    }

    .container {
        position: relative;
        top: 10px;
    }
    ul {
        position: relative;
        top: 20px;
    }
</style>