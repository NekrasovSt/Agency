<template>
  <div id="app" class="mdl-layout mdl-js-layout mdl-layout--fixed-header">
    <header class="mdl-layout__header mdl-layout__header--waterfall portfolio-header">
      <div class="mdl-layout__header-row portfolio-logo-row">
                <span class="mdl-layout__title">
                    <div class="portfolio-logo"></div>
                    <span class="mdl-layout__title">Simple portfolio website</span>
                </span>
      </div>
      <div class="mdl-layout__header-row portfolio-navigation-row mdl-layout--large-screen-only">
        <nav class="mdl-navigation mdl-typography--body-1-force-preferred-font">
          <router-link class="mdl-navigation__link" to="/main" active-class="is-active">Главная</router-link>
          <router-link class="mdl-navigation__link" to="/certificate" active-class="is-active"
                       v-if="!auth.isAuthenticated">Сертификаты
          </router-link>
          <router-link class="mdl-navigation__link" to="/mortgage" active-class="is-active"
                       v-if="!auth.isAuthenticated">Ипотека
          </router-link>
          <router-link class="mdl-navigation__link" to="/support" active-class="is-active" v-if="!auth.isAuthenticated">
            Сопровождение
          </router-link>
          <router-link class="mdl-navigation__link" to="/edit-announcement" v-if="auth.isAuthenticated">Объявление
          </router-link>
          <router-link class="mdl-navigation__link" to="/edit-object" v-if="auth.isAuthenticated">Объект недвижимости
          </router-link>
          <button class="mdl-navigation__link menu-btn" v-on:click="logout()" v-if="auth.isAuthenticated">
            Выход
          </button>
        </nav>
      </div>
    </header>
    <div class="mdl-layout__drawer mdl-layout--small-screen-only">
      <nav class="mdl-navigation mdl-typography--body-1-force-preferred-font">
        <router-link class="mdl-navigation__link" to="/">Главная</router-link>
        <a class="mdl-navigation__link" href="blog.html">Blog</a>
        <a class="mdl-navigation__link" href="about.html">About</a>
        <a class="mdl-navigation__link" href="contact.html">Contact</a>
      </nav>
    </div>
    <main class="mdl-layout__content">
      <router-view/>
    </main>
  </div>
</template>

<script>
  import auth from '@/miscellaneous/auth'
  import axios from 'axios';

  export default {
    name: 'App',
    data() {
      return {
        auth: auth
      }
    },
    methods: {
      logout() {
        this.auth.accessToken = null;
      }
    },
    created() {
      let self = this;
      axios.interceptors.request.use((config) => {
        if (auth.isAuthenticated) {
          Object.assign(config.headers, auth.header);
        }
        return config;
      });
      axios.interceptors.response.use((config) => config, error => {
        if (error.response.status === 401) {
          auth.accessToken = null;
          self.$router.push({name: 'login'});
        }
        return Promise.reject(error);
      });
    },
    beforeDestroy() {
      axios.interceptors.request.handlers.length = 0;
      axios.interceptors.response.handlers.length = 0;
    }
  }
</script>

<style>
  .menu-btn {
    border: none;
    background-color: transparent;
  }
</style>
