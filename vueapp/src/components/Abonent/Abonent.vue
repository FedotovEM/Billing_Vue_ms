<template>
    <div @click.left="clearSelect()">
        <h1>Абоненты</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>ФИО</option>
            <option>Улица</option>
            <option>Номер дома</option>
            <option>Номер квартиры</option>
            <option>Телефон</option>
            <option>Корпус</option>
            <option>Район</option>
            <option>Количество зарегистрированных жителей</option>
            <option>Размер помещения (м2)</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/abonent/add">Добавить новую запись</RouterLink>
        </div>
        <div class="table-responsive">
            <table id="table_id" class="table table-hover table-bordered" style="width:100%">
                <thead class="thead-light">
                    <tr>
                        <th scope="col-sm-1" data-type="int" @click="sort($event, 'accountCd')">Лицевой счет 
                            <span v-if="sortedBy === 'accountCd' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'accountCd' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'accountCd' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'accountCd'">⇅</span></th>
                        <th class="col-sm-3" scope="col" data-type="string" @click="sort($event, 'fio')">ФИО 
                            <span v-if="sortedBy === 'fio' && reverse === 0">⇅</span>
                            <span v-if="sortedBy === 'fio' && reverse === 1">🠅</span>
                            <span v-if="sortedBy === 'fio' && reverse === -1">🠇</span>
                            <span v-if="sortedBy !== 'fio'">⇅</span></th>
                        <th class="col-sm-3" scope="col" data-type="string" @click="sort($event, 'streetName')">Улица 
                            <span v-if="sortedBy === 'streetName' && reverse === 0">⇅</span>
                            <span v-if="sortedBy === 'streetName' && reverse === 1">🠅</span>
                            <span v-if="sortedBy === 'streetName' && reverse === -1">🠇</span>
                            <span v-if="sortedBy !== 'streetName'">⇅</span></th>
                        <th class="col-sm-1" scope="col" data-type="int" @click="sort($event, 'houseNo')">Номер дома 
                            <span v-if="sortedBy === 'houseNo' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'houseNo' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'houseNo' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'houseNo'">⇅</span></th>
                        <th class="col-sm-1" scope="col" data-type="int" @click="sort($event, 'flatNo')">Номер квартиры 
                            <span v-if="sortedBy === 'flatNo' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'flatNo' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'flatNo' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'flatNo'">⇅</span></th>
                        <th scope="col-sm-1" data-type="string" @click="sort($event, 'phone')">Телефон 
                            <span v-if="sortedBy === 'phone' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'phone' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'phone' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'phone'">⇅</span></th>
                        <th scope="col-sm-1" data-type="int" @click="sort($event, 'corpus')">Корпус 
                            <span v-if="sortedBy === 'corpus' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'corpus' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'corpus' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'corpus'">⇅</span></th>
                        <th class="col-sm-2" scope="col" data-type="string" @click="sort($event, 'district')">Район 
                            <span v-if="sortedBy === 'district' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'district' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'district' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'district'">⇅</span></th>
                        <th class="col-sm-1" scope="col" data-type="int" @click="sort($event, 'countPerson')">Количество зарегистрированных жителей
                            <span v-if="sortedBy === 'countPerson' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'countPerson' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'countPerson' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'countPerson'">⇅</span>
                        </th>
                        <th class="col-sm-1" scope="col" data-type="double" @click="sort($event, 'sizeFlat')">Размер помещения (м2)
                            <span v-if="sortedBy === 'sizeFlat' && reverse == 0">⇅</span>
                            <span v-if="sortedBy === 'sizeFlat' && reverse == 1">🠅</span>
                            <span v-if="sortedBy === 'sizeFlat' && reverse == -1">🠇</span>
                            <span v-if="sortedBy !== 'sizeFlat'">⇅</span>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in abonentList" :key="item.accountCd" @contextmenu.prevent="showActions($event, item)">
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
                    </tr>
                </tbody>
            </table>
            <ActionsMenu v-if="showMenu" :itemId="selectedItem.accountCd" :items="abonentsLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
        </div>
    </div>
</template>
<script>
    import { tableSort } from '../../sort.js';
    import { tableFiltr } from '../../filtration.js';
    import ActionsMenu from '../../components/helpful/ActionsMenu.vue';

    export default {
        data() {
            return {
                showMenu: false,
                selectedItem: null, 
                x: 0,
                y: 0,
                reverse: 0,
                sortedBy: '',
                colIndex: -1,
                abonentsLink: [
                    { title: 'Внести платёж', nameComponent: 'abonentPay' },
                    { title: 'История оплаты и начисления', nameComponent: 'abonentPayNachislHist' },
                    { title: 'История измений', nameComponent: 'abonentHist' },
                    { title: 'Карточка абонента', nameComponent: 'abonentCard' },
                    { title: 'Редактировать', nameComponent: 'editabonent' },
                    { title: 'Удалить', nameComponent: 'delabonent' },
                ]
            };
        },
        components: {
            ActionsMenu
        },
        methods: {
            showActions(event, item) {
                this.selectedItem = item;
                this.showMenu = true;
                this.x = event.clientX + window.pageXOffset;
                this.y = event.clientY + window.pageYOffset;
            },
            clearSelect() {
                this.showMenu = false;
            },
            sort(event, columnName) {
                if (this.sortedBy !== columnName) {
                    this.sortedBy = columnName
                    this.reverse = 0
                }

                this.colIndex = tableSort(event, this.colIndex);
                if (this.reverse == -1) {
                    this.reverse = 1;
                }
                else
                    this.reverse = -1;
            },
            filter() {
                tableFiltr();
            }
        }
    }
</script>

<script setup>
    import { onMounted, ref } from "vue";
    import axios from 'axios';
    import authHeader from '../../services/auth-header.js';
    import { urls } from '../../settings.js';

    const abonentList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/Abonents", { headers: authHeader() })
            .then((response) => {
                abonentList.value = response.data;
            })
    })
</script>

<style>
    table {
        font-size: 0.9em; /* уменьшить размер букв */
    }
    table tr {
        line-height: 0.8; /* уменьшить отступы между строками */
    }
    table td {
        padding: 2px; /* уменьшить отступы в ячейках */
    }
    .addButton {
        position: relative;
        margin-top: 20px;
    }
    select.form-select-sm {
        position: relative;
        display: inline-block;
        width: 300px;
        height: 32px;
    }
    input.form-control-sm {
        position: relative;
        display: inline-block;
        width: 300px;
        left: 20px;
    }

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

    th {
        cursor: pointer;
    }
</style>