<template>
    <div class="container">
        <header class="jumbotron">
            <h3>
                Профиль <strong>{{currentUser.username}}</strong>
            </h3>
        </header>
        <p>
            <strong>Токен:</strong>
            {{currentUser.token.substring(0, 20)}} ... {{currentUser.token.substr(currentUser.token.length - 20)}}
        </p>
        <p>
            <strong>Id: </strong>{{currentUser.id}}
        </p>
        <p>
            <strong>Email: </strong> {{currentUser.email}}
        </p>
        <p>
            <strong>Роль: </strong>{{currentUser.roles}}
        </p>
    </div>
</template>

<script>
    export default {
        name: 'Profile',
        computed: {
            currentUser() {
                return this.$store.state.auth.user;
            }
        },
        mounted() {
            if (!this.currentUser) {
                this.$router.push('/login');
            }
        }
    };
</script>

<style>
    .container {
        position: relative;
        top: 20px;
    }
</style>