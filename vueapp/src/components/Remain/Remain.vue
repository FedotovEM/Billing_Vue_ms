<template>
    <div @click.left="clearSelect()">
        <h1>Остатки</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>Услуга</option>
            <option>Месяц расчета остатка</option>
            <option>Год расчета остатка</option>
            <option>Сумма остатка</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/remain/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-1" data-type="int" @click="sort">#</th>
                    <th class="col-sm-1" scope="col" data-type="int">Лицевой счет</th>
                    <th class="col-sm-5" scope="col" data-type="string">Услуга</th>
                    <th class="col-sm-2" scope="col" data-type="int">Месяц расчета остатка</th>
                    <th class="col-sm-2" scope="col" data-type="int">Год расчета остатка</th>
                    <th class="col" scope="col" data-type="double">Сумма остатка</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in itemList" :key="item.remainCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.remainCd}}</th>
                    <td> {{item.accountCd}} </td>
                    <td> {{item.serviceName}} </td>
                    <td> {{item.remmonth}} </td>
                    <td> {{item.remyear}} </td>
                    <td> {{item.remainsum}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.remainCd" :items="remainsLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                remainsLink: [
                    { title: 'Редактировать', nameComponent: 'editremain' },
                    { title: 'Удалить', nameComponent: 'delremain' },
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
        axios.get(urls.webapi + "/Remains", { headers: authHeader() })
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