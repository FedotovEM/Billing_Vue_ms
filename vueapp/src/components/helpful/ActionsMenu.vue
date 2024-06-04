<template>
    <div class="actions-menu">        
        <ul>
            <li v-for="(item, i) in items" :key="i" @click="goToComponent(item.nameComponent)">{{ item.title }}</li>
        </ul>
    </div>
</template>

<script>
    export default {
        props: ['itemId', 'items'],
        methods: {
            goToComponent(nameComponent) {
                this.$router.push({ name: nameComponent, params: { id: this.itemId } })
            },
        },
    };
</script>

<style>
    .actions-menu {
        position: absolute;
        background-color: #fff;
        border: 1px solid #ddd;
        padding: 20px;
    }

        .actions-menu ul {
            list-style: none;
            margin: 0;
            padding: 0;
            top: 0;
        }

    .actions-menu li {
        cursor: pointer;
    }
    .actions-menu li:hover {
        background-color: #808080; 
        color: #fff; 
    }
</style>