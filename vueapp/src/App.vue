<template>
    <div id="app">
        <nav class="navbar navbar-expand navbar-dark bg-dark">
            <div class="navbar-nav mr-auto">
                <link class="nav-item">
                <router-link to="/" class="nav-link"><font-awesome-icon icon="home"></font-awesome-icon> Главная </router-link>
                
                <link v-if="showAdminBoard" class="nav-item">
                <router-link to="/admin" class="nav-link"> Панель оператора </router-link>
                
                <!--<link class="nav-item">
                <router-link v-if="currentUser" to="/user" class="nav-link"> Пользователь </router-link>-->
            </div>
            <div v-if="!currentUser" class="navbar-nav ml-auto">
                <link class="nav-item">
                <router-link to="/register" class="nav-link"><font-awesome-icon icon="user-plus" /> Зарегистрироваться </router-link>
                <link class="nav-item">
                <router-link to="/login" class="nav-link"><font-awesome-icon icon="sign-in-alt" /> Войти </router-link>
            </div>
            <div v-if="currentUser" class="navbar-nav ml-auto">
                <link class="nav-item">
                <router-link to="/profile" class="nav-link">
                    <font-awesome-icon icon="user" />
                    {{ currentUser.username }}
                </router-link>
                <link class="nav-item">
                <a class="nav-link" @click.prevent="logOut"><font-awesome-icon icon="sign-out-alt" /> Выйти </a>
            </div>
        </nav>

        <div>
            <router-view />
        </div>
    </div>
</template>

<script>
export default {
  computed: {
    currentUser() {
      return this.$store.state.auth.user;
    },
    showAdminBoard() {
      if (this.currentUser && this.currentUser['roles']) {
        return this.currentUser['roles'].includes('ROLE_ADMIN');
      }

      return false;
    },
    showModeratorBoard() {
      if (this.currentUser && this.currentUser['roles']) {
        return this.currentUser['roles'].includes('ROLE_MODERATOR');
      }

      return false;
    }
  },
  methods: {
    logOut() {
      this.$store.dispatch('auth/logout');
      this.$router.push('/login');
    }
  }
};
</script>