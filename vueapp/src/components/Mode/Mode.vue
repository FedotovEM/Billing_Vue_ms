<template>
    <div @click.left="clearSelect()">
        <h1>Режимы потребления</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Режим потребления</option>
            <option>Норма</option>
            <option>Услуга</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/mode/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-1" data-type="int" @click="sort">#</th>
                    <th class="col" scope="col" data-type="string">Режим потребления</th>
                    <th class="col-sm-1" scope="col" data-type="double">Норма</th>
                    <th class="col-sm-2" scope="col" data-type="string">Услуга</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in modeList" :key="item.modeCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.modeCd}}</th>
                    <td> {{item.modeName}} </td>
                    <td> {{item.norma}} </td>
                    <td> {{item.serviceName}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.modeCd" :items="modesLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                modesLink: [
                    { title: 'Редактировать', nameComponent: 'editmode' },
                    { title: 'Удалить', nameComponent: 'delmode' },
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

    const modeList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Modes", { headers: authHeader() })
            .then((response) => {
                modeList.value = response.data;
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