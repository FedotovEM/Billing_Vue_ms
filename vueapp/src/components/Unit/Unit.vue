<template>
    <div @click.left="clearSelect()">
        <h1>Единицы измерения</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Единицы измерения</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/unit/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col" data-type="int" @click="sort">#</th>
                    <th scope="col" data-type="string">Единица измерения</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in unitList" :key="item.unitCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.unitCd}}</th>
                    <td> {{item.unitsName}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.unitCd" :items="unitsLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                unitsLink: [
                    { title: 'Редактировать', nameComponent: 'editunit' },
                    { title: 'Удалить', nameComponent: 'delunit' },
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

    const unitList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Units", { headers: authHeader() })
            .then((response) => {
                unitList.value = response.data;
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