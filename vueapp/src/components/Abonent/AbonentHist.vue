<template>
    <div class="paysD">
        <form @submit.prevent="postHistReq">
            <h4 class="form-label">Лицевой счет</h4>
            <input type="text" disabled="disabled" class="form-control" v-model="histReq.accountCd">

            <h4>Просмотреть историю с</h4>
            <label class="form-label">Даты начала</label>
            <div class="form-group">
                <input type="date" class="form-control" v-model="histReq.startDate">
            </div>
            <label class="form-label">по конечную дату</label>
            <div class="datetime-picker-container">
                <input type="date" class="form-control" v-model="histReq.endDate">
                <button type="submit" class="btn btn-primary">Найти</button>
            </div>    
                <div class="table-responsive">
                    <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                        <thead class="thead-light">
                            <tr>
                                <th scope="col" data-type="int">Лицевой счет</th>
                                <th class="col-sm-3" scope="col" data-type="string">ФИО</th>
                                <th class="col-sm-1" scope="col" data-type="string">Улица</th>
                                <th class="col-sm-1" scope="col" data-type="int">Номер дома</th>
                                <th class="col-sm-1" scope="col" data-type="int">Номер квартиры</th>
                                <th scope="col" data-type="string">Телефон</th>
                                <th scope="col" data-type="int">Корпус</th>
                                <th class="col-sm-1" scope="col" data-type="string">Район</th>
                                <th class="col-sm-1" scope="col" data-type="int">Количество зарегистрированных жителей</th>
                                <th class="col-sm-1" scope="col" data-type="double">Размер помещения (м2)</th>
                                <th class="col-sm-1" scope="col" data-type="date">Действительна с</th>
                                <th class="col-sm-1" scope="col" data-type="date">Действительна по</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in abonentHist" :key="item.serviceCd">
                                <td> {{item.accountCd}} </td>
                                <td> {{item.fio}} </td>
                                <td> {{item.streetName}} </td>
                                <td> {{item.houseNo}} </td>
                                <td> {{item.flatNo}} </td>
                                <td> {{item.phone}} </td>
                                <td> {{item.corpus}} </td>
                                <td> {{item.district}} </td>
                                <td> {{item.countPerson}} </td>
                                <td> {{item.sizeFlat}} </td>
                                <td> {{item.startDate.replace('T00:00:00','')}} </td>
                                <td v-if="item.endDate">{{item.endDate.replace('T00:00:00','')}} </td>
                                <td v-if="!item.endDate"></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
        </form>
    </div>
    
</template>

<script>
    export default {
        props: {
            searchAccountcd: String
        }
    }
</script>


<script setup>
    import { onMounted, reactive, ref } from "vue";
    import { useRoute, useRouter, RouterLink } from "vue-router";
    import axios from 'axios';
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';
    
    const route = useRoute();
    const router = useRouter();

    let histReq = reactive({
        accountCd: route.params.id,
        startDate: null,
        endDate: null
    });

    const abonentHist = ref([])
    
    onMounted(() => {
        axios.get(urls.webapi + `/Abonents/abonent-hist/${route.params.id}`, { headers: authHeader() })
            .then((response) => {
                abonentHist.value = response.data;
            })
    })

    const postHistReq = async () => {
        axios.post(urls.webapi + "/Abonents/get-abonent-hist", histReq, { headers: authHeader() })
            .then((response) => {
                abonentHist.value = response.data;
            })
    }
    
</script>

<style>
    select.form-select {
        width: 400px;
    }    
    .form-control {
        position: relative;
        width: 400px;
    }
    .container {
        position: relative;
        top: 10px;
    }
    .paysD {
        position: absolute;
        margin-top: 20px;
    }
    th {
        cursor: pointer;
    }

    .datetime-picker-container {
        display: flex;
        align-items: center;
    }

        .datetime-picker-container .btn {
            margin-left: 10px;
        }
</style>