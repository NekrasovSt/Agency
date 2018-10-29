<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp ">
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Параметры фильтрации</h2>
      </div>
      <div class="mdl-card__supporting-text">
        <form action="#">
          <div class="mdl-textfield mdl-js-textfield">
            <input class="mdl-textfield__input" type="text" id="filter" v-model="filter">
            <label class="mdl-textfield__label" for="filter">Пушкина...</label>
          </div>
        </form>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="refresh()">найти
        </button>
      </div>
    </div>
    <table class="mdl-data-table mdl-js-data-table mdl-shadow--2dp mdl-cell mdl-cell--12-col">
      <thead>
      <tr>
        <th class="mdl-data-table__cell--non-numeric">Населенный пункт</th>
        <th class="mdl-data-table__cell--non-numeric">Улица</th>
        <th class="mdl-data-table__cell--non-numeric">Дом/Строение</th>
        <th>Площадь</th>
        <th>Комнат</th>
        <th>+</th>
      </tr>
      </thead>
      <tbody>
      <tr v-for="item in data">
        <td class="mdl-data-table__cell--non-numeric">{{item.City}}</td>
        <td class="mdl-data-table__cell--non-numeric">{{item.Street}}</td>
        <td class="mdl-data-table__cell--non-numeric">{{item.Building}}</td>
        <td>{{item.Square}}</td>
        <td>{{item.Rooms}}</td>
        <td>
          <router-link class="mdl-button mdl-js-button mdl-button--icon" title="Редактировать"
                       :to="'/edit-object/' +item.Id">
            <i class="material-icons">edit</i>
          </router-link>
          <button class="mdl-button mdl-js-button mdl-button--icon" title="Удалить">
            <i class="material-icons">delete</i>
          </button>
          <router-link class="mdl-button mdl-js-button mdl-button--icon" title="Создать объявление"
                       :to="'/edit-announcement?basedOn=' + item.Id">
            <i class="material-icons">new_releases</i>
          </router-link>
        </td>
      </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
  import axios from 'axios';

  const perPage = 100;
  export default {
    name: "RealEstateObjectList",
    data() {
      return {
        top: perPage,
        skip: 0,
        data: [],
        count: 0,
        filter: null
      }
    },
    methods: {
      refresh() {
        let url = `odata/RealEstateObject?`;
        let segments = [];
        segments.push(`$top=${this.top}`);
        segments.push(`$skip=${this.skip}`);
        segments.push(`$count=true`);
        if (this.filter) {
          segments.push(`$filter=contains(toLower(Street),toLower('${this.filter}'))`);
        }
        url = url + segments.join('&');
        axios.get(url).then(response => {
          this.data = response.data.value;
          this.count = response.data['@odata.count'];
        });
      },
      next() {
        this.skip += perPage;
        this.refresh();
      },
      back() {
        this.skip -= perPage;
        this.refresh();
      }
    },
    computed: {
      canNext() {
        return this.count > this.top + this.skip;
      },
      canBack() {
        return this.skip > 0;
      }
    },
    created() {
      this.refresh();
    }
  }
</script>

<style scoped>

</style>
