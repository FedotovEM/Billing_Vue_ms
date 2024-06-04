<template>
    <nav>
        <div v-for="(item, i) in menuItems" :key="i">
            <DropDown :title=item.title :items=item.array />
        </div>
        <!--<div class="menu-item">
        <RouterLink to="/uploadFile">Загрузить файл</RouterLink>
    </div>-->
    </nav>
</template>

<script>
    import DropDown from './Dropdown';

    export default {
        name: 'nav-bar',
        props: ['menuItems'],
        components: {
            DropDown
        },
    }
</script>

<style>
    nav {
        display: flex;
        align-items: center;
    }
    nav .menu-item{
        color: #FFF;
        padding: 10px 20px;
        position: relative;
        text-align: center;
        border-bottom: 3px solid transparent;
        display: flex;
        transition: 0.4s;
    }

    nav .menu-item.active,
    nav .menu-item:hover{
        background-color: #444;
        border-bottom-color: #0026ff
    }

    nav .menu-item a{
        color: inherit;
        text-decoration: none;
    }
</style>