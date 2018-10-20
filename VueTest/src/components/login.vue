<template>

  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h1 class="mdl-card__title-text">Вход</h1>
      </div>
      <form class="mdl-card__supporting-text mdl-grid mdl-cell--4-col" name="object-from">
        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
          <input class="mdl-textfield__input" type="text" id="login" v-model="login" required>
          <label class="mdl-textfield__label" for="login">Имя</label>
        </div>
        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
          <input class="mdl-textfield__input" type="password" id="password" v-model="password" required>
          <label class="mdl-textfield__label" for="password">Пароль</label>
        </div>
      </form>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="send()"
          v-bind:disabled="isInvalid">
          вход
        </button>
        <button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect">
          Отмена
        </button>
      </div>
    </div>
    <div ref="snack" class="mdl-js-snackbar mdl-snackbar">
      <div class="mdl-snackbar__text"></div>
      <button class="mdl-snackbar__action" type="button"></button>
    </div>
  </div>


</template>

<script>
  import axios from 'axios';
  import auth from '@/miscellaneous/auth'

  export default {
    name: "Login",
    data() {
      return {
        login: null,
        password: null
      };
    },
    computed: {
      isInvalid() {
        return !this.login || !this.password;
      }
    },
    methods: {
      send() {
        let promise = axios.post('/Token', {Login: this.login, Password: this.password});
        promise.then((response) => {
          auth.accessToken = response.data;

          this.$router.push({name: 'objectList'});
        }).catch((error) => {
          this.$refs.snack.MaterialSnackbar ? this.$refs.snack.MaterialSnackbar.showSnackbar({message: 'Произошла ошибка!'}) : 0;
        });
      }
    },
    mounted() {
      if (window.componentHandler)
        componentHandler.upgradeAllRegistered();
    }
  }
</script>

<style scoped>

</style>
