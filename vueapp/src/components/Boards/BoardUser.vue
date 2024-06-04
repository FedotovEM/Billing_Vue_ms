<template>
    <div id="app">
        <header style="background-color: #3f3e3e">
            <h1 class="text-center" style="color: white">Учёт абонентов коммунального биллинга</h1>
            <div class="centered">
                <Navbar :menuItems="menuItems" />
            </div>
        </header>
        <div>
            <div id="app">
                <RouterView />
            </div>
        </div>
    </div>
</template>

<script>
    import UserService from "../../services/user.service";
    import Navbar from '../../components/helpful/Navbar';

    export default {
        name: "User",
        components: {
            Navbar
        },
        data() {
            return {
                menuItems: {
                    abonent: {
                        title: "Абонент",
                        array: [
                            { title: 'Карточка абонента', link: '/nachisl' },
                            { title: 'История измений', link: '/abonentSearchHist' },
                        ]
                    },
                    pay: {
                        title: "Провести платёж",
                        array: [
                            { title: 'Оплатить услугу', link: '/pay' },
                        ]
                    },

                }
            }
        },
        mounted() {
            UserService.getAdminBoard().then(
                (response) => {
                    this.content = response.data;
                },
                (error) => {
                    this.content =
                        (error.response &&
                            error.response.data &&
                            error.response.data.message) ||
                        error.message ||
                        error.toString();
                }
            );
        },
    };
</script>

<style>
    .text-center {
        text-align: center;
    }
    .centered {
        display: flex;
        justify-content: center;
        align-items: center;
    }
</style>