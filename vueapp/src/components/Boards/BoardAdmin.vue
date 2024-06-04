<template>
    <div id="app">
        <header style="background-color: #3f3e3e">
            <h1 class="text-center" style="color: white">Учёт абонентов коммунального биллинга</h1>
            <div class="centered">
                <Navbar :menuItems="menuItems" />
            </div>
        </header>
    </div>
</template>

<script>
    import UserService from "../../services/user.service";
    import Navbar from '../../components/helpful/Navbar';
    
export default {
        name: "Admin",
        components: {
            Navbar
        },
        data() {
            return {
                menuItems: {
                    abonents: {
                        title: "Абоненты",
                        array: [
                            { title: 'Абоненты', link: '/abonent' },
                            { title: 'Режимы потребления абонентов', link: '/abonentMode' },
                            { title: 'Улицы', link: '/street' },
                        ]
                    },
                    requests: {
                        title: "Заявки на ремонт",
                        array: [
                            { title: 'Заявки на ремонт', link: '/request' },
                            { title: 'Исполнители', link: '/executor' },
                            { title: 'Неисправности', link: '/disrepair' },
                        ]
                    },
                    pays: {
                        title: "Платежи",
                        array: [
                            { title: 'Оплачено', link: '/paySumma' },
                            { title: 'Пункты приёма платежей', link: '/receptionPoint' },
                            { title: 'Остатки', link: '/remain' },
                        ]
                    },
                    nachisls: {
                        title: "Начисления",
                        array: [
                            { title: 'Начислено', link: '/nachislSumma' },
                            { title: 'Предоставляемые услуги', link: '/service' },
                            { title: 'Единицы измерения', link: '/unit' },
                            { title: 'Режимы потребления', link: '/mode' },
                            { title: 'Цены', link: '/price' },
                            { title: 'Остатки', link: '/remain' },
                        ]
                    },
                    reports: {
                        title: "Отчёты", 
                        array: [
                            { title: 'Отчет о ремонтных заявках за месяц', link: '/requestRepMonth' },
                            { title: 'ОСВ с расшифровкой по каждому абоненту', link: '/OSVEachAbonent' },
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