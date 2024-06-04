<template>
    <div @click.left="clearSelect()">
        <h1>Начислено</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>Услуга</option>
            <option>Сумма начисления</option>
            <option>Год начисления</option>
            <option>Месяц начисления</option>
            <option>Тип начисления</option>
            <option>Количество ресурсов</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/nachislSumma/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-1" data-type="int" @click="sort">#</th>
                    <th class="col-sm-2" scope="col" data-type="int">Лицевой счет</th>
                    <th class="col-sm-6" scope="col" data-type="string">Услуга</th>
                    <th class="col-sm-1" scope="col" data-type="double">Сумма начисления</th>
                    <th class="col-sm-1" scope="col" data-type="int">Год начисления</th>
                    <th class="col-sm-1" scope="col" data-type="int">Месяц начисления</th>
                    <th class="col-sm-1" scope="col" data-type="string">Тип начисления</th>
                    <th class="col-sm-1" scope="col" data-type="double">Количество ресурсов</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in nachislSummasList" :key="item.nachislFactCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.nachislFactCd}}</th>
                    <td> {{item.accountCd}} </td>
                    <td> {{item.serviceName}} </td>
                    <td> {{item.nachislSum}} </td>
                    <td> {{item.nachislYear}} </td>
                    <td> {{item.nachislMonth}} </td>
                    <td> {{item.nachType}} </td>
                    <td> {{item.countResources}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.nachislFactCd" :items="nachislsLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                nachislsLink: [
                    { title: 'Редактировать', nameComponent: 'editnachislSumma' },
                    { title: 'Удалить', nameComponent: 'delnachislSumma' },
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

    const nachislSummasList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/NachislSummas", { headers: authHeader() })
            .then((response) => {
                nachislSummasList.value = response.data;
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