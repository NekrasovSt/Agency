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
          <router-link class="mdl-navigation__link" to="/certificate" active-class="is-active">Сертификаты</router-link>
          <router-link class="mdl-navigation__link" to="/mortgage" active-class="is-active">Ипотека</router-link>
          <router-link class="mdl-navigation__link" to="/support" active-class="is-active">Сопровождение</router-link>
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

  axios.interceptors.request.use((config) => {
    if (auth.isAuthenticated) {
      Object.assign(config.headers, auth.header);
    }
    return config;
  }, function (error) {
    // Do something with request error
    return Promise.reject(error);
  });

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
    }
  }
</script>

<style>
  .menu-btn {
    border: none;
    background-color: transparent;
  }
</style>
