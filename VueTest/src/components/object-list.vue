<template>

  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-js-progress mdl-progress--accent"
         style="height: 4px"
         v-bind:class="{'mdl-progress mdl-progress__indeterminate': loading}"></div>
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp ">
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Параметры поиска</h2>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <!--Тип недвижимости-->
        <div class="mdl-cell mdl-cell--4-col">
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" v-bind:for="key"
                 v-for="(value, key) in defaultTypes">
            <input type="checkbox" v-bind:id="key" class="mdl-checkbox__input" v-bind:value="key" v-model="types">
            <span class="mdl-checkbox__label">{{value}}</span>
          </label>
        </div>
        <!--Количество комнат-->
        <div class="mdl-cell mdl-cell--4-col">
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room1">
            <input type="checkbox" id="room1" class="mdl-checkbox__input" value="1" v-model="rooms">
            <span class="mdl-checkbox__label">1 комната</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room2">
            <input type="checkbox" id="room2" class="mdl-checkbox__input" value="2" v-model="rooms">
            <span class="mdl-checkbox__label">2 комнаты</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room3">
            <input type="checkbox" id="room3" class="mdl-checkbox__input" value="3" v-model="rooms">
            <span class="mdl-checkbox__label">3 комнаты</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room4">
            <input type="checkbox" id="room4" class="mdl-checkbox__input" value="4" v-model="rooms">
            <span class="mdl-checkbox__label">4 и более комнат</span>
          </label>
        </div>
        <div class="mdl-cell mdl-cell--4-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="number" pattern="^[0-9]*" id="priceFrom" min="0"
                   v-model.number="priceFrom">
            <label class="mdl-textfield__label" for="priceFrom">цена от</label>
            <span class="mdl-textfield__error">Должно быть число больше 0!</span>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="number" pattern="^[0-9]*" id="priceTo" min="0"
                   v-model.number="priceTo">
            <label class="mdl-textfield__label" for="priceTo">цена до</label>
            <span class="mdl-textfield__error">Должно быть число больше 0!</span>
          </div>
        </div>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="refresh()"
          v-bind:disabled="isInvalid">
          найти
        </button>
      </div>
    </div>
    <div class="mdl-cell mdl-grid  mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp"
         v-for="item in announcements">
      <div class="mdl-card__media mdl-cell mdl-cell--12-col-tablet">
        <img class="article-image" v-bind:src="getUrl(item)" border="0" alt="">
      </div>
      <div class="mdl-cell mdl-cell--8-col">
        <h2 class="mdl-card__title-text">{{item.RealEstateObject.Rooms}} ком. {{item.RealEstateObject.Square}} кв.м.
          {{item.Price}} руб.</h2>
        <div class="mdl-card__supporting-text padding-top">
          <span>{{item.creationDate|format}}</span>
        </div>
        <div class="mdl-card__supporting-text no-left-padding">
          <p class="description-section">{{item.RealEstateObject.Description}}</p>
        </div>
        <div class="mdl-grid mdl-card__actions">
          <span class="mdl-chip">
            <span class="mdl-chip__text">Категория: {{item.RealEstateObject.RealEstateType | estate-type}}</span>
          </span>
          <div class="mdl-layout-spacer"></div>
          <router-link :to="'/announcement/' + item.Id"
                       class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent">
            Подробнее
          </router-link>
          <router-link :to="'/edit-announcement/'+item.Id" v-if="auth.isAuthenticated"
                       class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent">
            Редактировать
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import {realEstateTypes, count} from '../miscellaneous/real-estate-types';
  import auth from '@/miscellaneous/auth'

  export default {
    name: 'ObjectList',
    data() {
      return {
        defaultTypes: realEstateTypes,
        announcements: null,
        types: [],
        rooms: [],
        priceFrom: 0,
        priceTo: null,
        loading: false,
        auth: auth
      };
    },
    computed: {
      isInvalid() {
        if (isNaN(this.priceFrom) || isNaN(this.priceTo))
          return true;

        return this.priceTo !== '' && this.priceFrom !== '' &&
          this.priceTo !== null && this.priceFrom !== null &&
          this.priceFrom > this.priceTo;
      },
    },
    methods: {
      getUrl(obj) {
        if (obj.RealEstateObject && obj.RealEstateObject.RealEstateObjectFile.length !== 0) {
          return `/photos/${obj.RealEstateObject.RealEstateObjectFile[0].Name}`
        }
      },
      refresh() {
        let url = `odata/Announcement?$expand=RealEstateObject`;

        let filter = [];
        if (this.rooms.length > 0 && this.rooms.length < 4) {
          filter.push('(' + this.rooms.map(j => `RealEstateObject/Rooms eq ${j}`).join(' or ') + ')');
        }
        if (this.priceFrom !== null && this.priceFrom !== '' && this.priceFrom != 0) {
          filter.push(`(Price ge ${this.priceFrom})`);
        }
        if (this.priceTo !== null && this.priceTo !== '') {
          filter.push(`(Price le ${this.priceTo})`);
        }
        if (this.types.length > 0 && this.types.length !== count) {
          filter.push('(' + this.types.map(i => `RealEstateObject/RealEstateType eq '${i}'`).join(' or ') + ')');
        }

        if (filter.length !== 0) {
          url += '&$filter=' + filter.join(' and ');
        }
        this.loading = true;
        axios.get(url).then(response => {
          this.announcements = response.data.value;
        }).catch(error => {

        }).finally(() => {
          setTimeout(() => {
            this.loading = false;
          }, 300);

        });
      },
    },
    created() {
      if (this.$route.params.type) {
        let type = this.$route.params.type.charAt(0).toUpperCase() + this.$route.params.type.slice(1);
        this.types.push(type);
      }
      this.refresh();
    },
    mounted() {
      if (window.componentHandler)
        componentHandler.upgradeAllRegistered();
    },
  };
</script>

<style scoped>
  .description-section {
    max-height: 200px;
    overflow: hidden;
    text-overflow: ellipsis;
    display: -webkit-box;
    -webkit-line-clamp: 3;
    -webkit-box-orient: vertical;
  }
</style>
