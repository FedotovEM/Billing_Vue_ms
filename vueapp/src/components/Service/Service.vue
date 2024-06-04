<template>
    <div @click.left="clearSelect()">
        <h1>Услуги</h1>
        <select id="IdCombobox" class="form-select-sm">
            <option>Услуга</option>
            <option>Единица измерения</option>
        </select>

        <input type="text" class="form-control-sm" id="Input" placeholder="Поиск" @keyup="filter" />

        <div class="p-3">
            <RouterLink class="btn btn-outline-primary" to="/service/add">Добавить новую запись</RouterLink>
        </div>
        <table id="table_id" class="table table-hover table-bordered" style="width:100%">
            <thead class="thead-light">
                <tr>
                    <th scope="col-sm-2" data-type="int" @click="sort">#</th>
                    <th class="col-sm-8" scope="col" data-type="string">Услуга</th>
                    <th class="col-sm-3" scope="col" data-type="string">Единица измерения</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="item in serviceList" :key="item.serviceCd" @contextmenu.prevent="showActions($event, item)">
                    <th scope="row">{{item.serviceCd}}</th>
                    <td> {{item.serviceName}} </td>
                    <td> {{item.unitsName}} </td>
                </tr>
            </tbody>
        </table>
        <ActionsMenu v-if="showMenu" :itemId="selectedItem.serviceCd" :items="serviceLink" @close="showMenu = false" :style="{ top: `${y}px`, left: `${x}px` }" />
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
                serviceLink: [
                    { title: 'Редактировать', nameComponent: 'editservice' },
                    { title: 'Удалить', nameComponent: 'delservice' },
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

    const serviceList = ref([])
    onMounted(() => {
        axios.get(urls.nachServ + "/Services", { headers: authHeader() })
            .then((response) => {
                serviceList.value = response.data;
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