<template>
    <div @click.left="clearSelect()">
        <h1>Заявки на ремонт</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>Неисправность</option>
            <option>ФИО исполнителя</option>
            <option>Поступление заявки</option>
            <option>Выполнение заявки</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/request/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-1" data-type="int" @click="sort">#</th>
                    <th class="col-sm-1" scope="col" data-type="int">Лицевой счет</th>
                    <th class="col-sm-5" scope="col" data-type="string">Неисправность</th>
                    <th class="col-sm-3" scope="col" data-type="string">ФИО исполнителя</th>
                    <th class="col-sm-1" scope="col" data-type="date">Поступление заявки</th>
                    <th class="col-sm-1" scope="col" data-type="date">Выполнение заявки</th>
                    <th class="col-sm-1" scope="col">Погашена</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in itemList" :key="item.requestCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.requestCd}}</th>
                    <td> {{item.accountCd}} </td>
                    <td> {{item.failureName}} </td>
                    <td> {{item.fio}} </td>
                    <td> {{item.incomingDate.replace('T00:00:00','')}} </td>
                    <td v-if="item.executionDate">{{item.executionDate.replace('T00:00:00','')}} </td>
                    <td v-if="!item.executionDate"></td>
                    <td v-if="item.executed">
                        <input type="checkbox" disabled="disabled" checked>
                    </td>
                    <td v-if="!item.executed">
                        <input type="checkbox" disabled="disabled">
                    </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.requestCd" :items="requestsLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                requestsLink: [
                    { title: 'Редактировать', nameComponent: 'editrequest' },
                    { title: 'Удалить', nameComponent: 'delrequest' },
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
            sort() {
                tableSort();
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

    const itemList = ref([])
    onMounted(() => {
        axios.get(urls.RepairReqServ + "/Requests", { headers: authHeader() })
            .then((response) => {
                itemList.value = response.data;
            })
    })

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
</style>