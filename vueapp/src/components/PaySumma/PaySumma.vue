<template>
    <div @click.left="clearSelect()">
        <h1>Оплачено</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>Услуга</option>
            <option>Пункт приема платежа</option>
            <option>Оплачиваемый год</option>
            <option>Оплачиваемый месяц</option>
            <option>Тип оплаты</option>
            <option>Дата оплаты</option>
            <option>Сумма оплаты</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/paySumma/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-1" data-type="int" @click="sort">#</th>
                    <th class="col-sm-1" scope="col" data-type="int">Лицевой счет</th>
                    <th class="col-sm-3" scope="col" data-type="string">Услуга</th>
                    <th class="col-sm-2" scope="col" data-type="string">Пункт приема платежа</th>
                    <th class="col-sm-1" scope="col" data-type="int">Год оплаты</th>
                    <th class="col-sm-1" scope="col" data-type="int">Месяц оплаты</th>
                    <th class="col-sm-1" scope="col" data-type="string">Тип оплаты</th>
                    <th class="col-sm-2" scope="col" data-type="date">Дата оплаты</th>
                    <th class="col-sm-2" scope="col" data-type="double">Сумма оплаты</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in paySummasList" :key="item.payFactCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.payFactCd}}</th>
                    <td> {{item.accountCd}} </td>
                    <td> {{item.serviceName}} </td>
                    <td> {{item.receptionName}} </td>
                    <td> {{item.payYear}} </td>
                    <td> {{item.payMonth}} </td>
                    <td> {{item.payType}} </td>
                    <td> {{item.payDate.replace('T00:00:00','')}} </td>
                    <td> {{item.paySum}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.payFactCd" :items="paysLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                paysLink: [
                    { title: 'Редактировать', nameComponent: 'editpaySumma' },
                    { title: 'Удалить', nameComponent: 'delpaySumma' },
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

    const paySummasList = ref([])
    onMounted(() => {
        axios.get(urls.payServ + "/PaySummas", { headers: authHeader() })
            .then((response) => {
                paySummasList.value = response.data;
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