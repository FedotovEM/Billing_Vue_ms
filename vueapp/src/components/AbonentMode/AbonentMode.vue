<template>
    <div @click.left="clearSelect()">
        <h1>Режимы потребления абонентов</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Лицевой счет</option>
            <option>Режим потребления</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter"/>

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/abonentMode/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col" data-type="int" @click="sort">#</th>
                    <th class="col-sm-1" scope="col" data-type="int">Лицевой счет</th>
                    <th class="col" scope="col" data-type="string">Режим потребления</th>
                    <th class="col-sm-1" scope="col">Наличие счетчика</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in abonentModeList" :key="item.abonentModeСd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.abonentModeСd}}</th>
                    <td> {{item.accountCd}} </td>
                    <td> {{item.modeName}} </td>
                    <td v-if="item.counterr">
                        <input type="checkbox" disabled="disabled" checked>
                    </td>
                    <td v-if="!item.counterr">
                        <input type="checkbox" disabled="disabled">
                    </td>                   
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.abonentModeСd" :items="abonentModesLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                abonentModesLink: [
                    { title: 'Редактировать', nameComponent: 'editabonentMode' },
                    { title: 'Удалить', nameComponent: 'delabonentMode' },
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

    const abonentModeList = ref([])
    onMounted(() => {
        axios.get(urls.webapi + "/AbonentModes", { headers: authHeader() })
            .then((response) => {
                abonentModeList.value = response.data;
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

    th {
        cursor: pointer;
    }
    select.form-select-sm {
        position: relative;
        display: inline-block;
        width: 300px;
        left: 6px;
        height: 32px;
    }

    input.form-control-sm {
        position: relative;
        display: inline-block;
        width: 300px;
        left: 20px;
        left: 6px;
    }
</style>