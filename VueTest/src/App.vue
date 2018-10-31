<template>
  <div id="app" class="mdl-layout mdl-js-layout mdl-layout--fixed-header">
    <header class="mdl-layout__header portfolio-header">
      <div class="mdl-layout__header-row portfolio-logo-row">
                <span class="mdl-layout__title">
                    <div class="portfolio-logo"></div>
                    <span class="mdl-layout__title">Чтобы продать что-нибудь ненужное, нужно сначала купить что-нибудь ненужное, а у нас денег нет</span>
                </span>
      </div>
      <div class="mdl-layout__header-row portfolio-navigation-row mdl-layout--large-screen-only">
        <nav class="mdl-navigation mdl-typography--body-1-force-preferred-font">
          <router-link class="mdl-navigation__link" to="/main" active-class="is-active">Главная</router-link>
          <router-link class="mdl-navigation__link" to="/certificate" active-class="is-active">Сертификаты
          </router-link>
          <router-link class="mdl-navigation__link" to="/mortgage" active-class="is-active">Ипотека
          </router-link>
          <router-link class="mdl-navigation__link" to="/support" active-class="is-active">
            Сопровождение
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
        <router-link class="mdl-navigation__link" to="/certificate" active-class="is-active">Сертификаты
        </router-link>
        <router-link class="mdl-navigation__link" to="/mortgage" active-class="is-active">Ипотека
        </router-link>
        <router-link class="mdl-navigation__link" to="/support" active-class="is-active">
          Сопровождение
        </router-link>
        <button class="mdl-navigation__link menu-btn" v-on:click="logout()" v-if="auth.isAuthenticated">
          Выход
        </button>
      </nav>
    </div>
    <main>
      <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-js-progress mdl-progress--accent"
           style="height: 4px"
           v-bind:class="{'mdl-progress mdl-progress__indeterminate': numLoadings !== 0}"></div>
      <router-view/>
      <footer class="mdl-mega-footer">
        <div class="mdl-mega-footer__middle-section">
          <div class="mdl-mega-footer__drop-down-section">
            <input class="mdl-mega-footer__heading-checkbox" type="checkbox" checked>
            <h1 class="mdl-mega-footer__heading">Главная</h1>
            <ul class="mdl-mega-footer__link-list">
              <li>
                <router-link to="/object-list/apartment">Квартиры</router-link>
              </li>
              <li>
                <router-link to="/object-list/newBuilding">Новостройки</router-link>
              </li>
              <li>
                <router-link to="/object-list/house">Дом</router-link>
              </li>
            </ul>
          </div>
          <div class="mdl-mega-footer__drop-down-section" v-if="auth.isAuthenticated">
            <input class="mdl-mega-footer__heading-checkbox" type="checkbox" checked>
            <h1 class="mdl-mega-footer__heading">Администрирование</h1>
            <ul class="mdl-mega-footer__link-list">
              <li>
                <router-link to="/real-estate-object-list">Объекты недвижимости</router-link>
              </li>
              <li>
                <router-link to="/object-list">Объявления</router-link>
              </li>
            </ul>
          </div>
        </div>
        <div class="mdl-mega-footer__bottom-section mdl-mega-footer__bottom-section_right">
          <div class="mdl-logo">By Nek</div>
        </div>
      </footer>
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
        auth: auth,
        numLoadings: 0
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
        self.numLoadings++;
        if (auth.isAuthenticated) {
          Object.assign(config.headers, auth.header);
        }
        return config;
      });
      axios.interceptors.response.use((config) => {
        self.numLoadings--;
        return config;
      }, error => {
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

  footer.mdl-mini-footer {
    flex-basis: 63px;
    flex-shrink: 0;
    box-sizing: border-box;
    padding: 16px;
  }

  main {
    background-color: white;
  }

  body {
    background-color: #424242;
  }

  .page-placeholder > div {
    flex-grow: 1;
  }

  .mdl-mega-footer__bottom-section_right > .mdl-logo {
    float: right;
  }
</style>
