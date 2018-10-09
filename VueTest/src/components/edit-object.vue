<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h1 class="mdl-card__title-text">Заполните форму</h1>
      </div>
      <div class="mdl-card__supporting-text  mdl-grid">
        <div class="mdl-cell mdl-cell--4-col mdl-card__media thumb" v-for="(file, index) in images">
          <img :src="source(file)" class="article-image thumb--image"/>
          <div class="mdl-card__menu">
            <button class="mdl-button mdl-button--icon mdl-js-button mdl-js-ripple-effect"
                    @click.prevent="remove(file)">
              <i class="material-icons">delete</i>
            </button>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--12-col">
          <label
            class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent">
            Загрузит файл
            <input type="file" class="file-input" ref="files" multiple v-on:change="handleFilesUpload($refs.files)"/>
          </label>
          <button type="button" @click="uploadServer()">Start upload
          </button>
        </div>
      </div>

      <form class="mdl-card__supporting-text mdl-grid">
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield">
            <v-select
              v-model="currentRegion"
              :filterable="false"
              label="name"
              @search="onRegionSearch"
              class="mdl-textfield mdl-input__expandable-holder"
              placeholder="Выберите край/область..."
              inputId="region"
              :options="regions">
            </v-select>
          </div>
          <div class="mdl-textfield mdl-js-textfield">
            <v-select
              v-model="currentCity"
              :disabled="!regionSelected"
              :filterable="false"
              label="name"
              placeholder="Выберите город..."
              @search="onCitySearch"
              :options="cities">
            </v-select>
          </div>
          <div class="mdl-textfield mdl-js-textfield">
            <v-select
              v-model="currentStreet"
              :disabled="!citySelected"
              :filterable="false"
              label="name"
              placeholder="Выберите улицу..."
              @search="onStreetSearch"
              :options="streets">
            </v-select>
          </div>
          <div class="mdl-textfield mdl-js-textfield">
            <v-select
              v-model="currentBuilding"
              :disabled="!streetSelected"
              :filterable="false"
              label="name"
              placeholder="Выберите номер дома/строения..."
              @search="onBuildingSearch"
              :options="buildings">
            </v-select>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield">
            <input class="mdl-textfield__input" type="text" id="floor" v-model="floor" required>
            <label class="mdl-textfield__label" for="floor">Этаж/Этажей</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield">
            <input class="mdl-textfield__input" type="number" id="square" v-model="square" required>
            <label class="mdl-textfield__label" for="square">Площадъ</label>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    name: 'EditObject',
    data() {
      return {
        currentRegion: null,
        currentCity: null,
        currentStreet: null,
        currentBuilding: null,
        square: null,
        regions: [],
        cities: [],
        streets: [],
        buildings: [],
        floor: null,
        images: [],
      };
    },
    computed: {
      regionSelected() {
        return !!this.currentRegion;
      },
      citySelected() {
        return !!this.currentCity;
      },
      streetSelected() {
        return !!this.currentStreet;
      },
      buildingSelected() {
        return !!this.currentBuilding;
      },
    },
    methods: {
      remove(file) {
        // this.images.find(i => i.name === file.name && i.size === file.size);
        let index = this.images.indexOf(file);
        this.images.splice(index, 1);
      },
      source(file) {
        console.log(URL, URL.createObjectURL);
        return URL.createObjectURL(file);
      },
      handleFilesUpload(event) {
        let files = event.files;//this.$refs.files.files;
        for (let i = 0; i < files.length; i++) {
          if (!this.images.some(
            j => j.name === files[i].name && j.size === files[i].size)) {
            this.images.push(files[i]);
          }
        }
      },
      uploadServer() {
        let formData = new FormData();
        this.images.forEach((i, j) => {
          formData.append(`file`, i);
        });
        const obj = {
          hello: 'world',
        };
        const json = JSON.stringify(obj);
        const blob = new Blob([json], {
          type: 'application/json',
        });
        formData.append('document', blob);
        axios.post('/odata/RealEstateObject', formData, {headers: {'Content-type': 'multipart/form-data'}}).
          then(function() {
            console.log('SUCCESS!!');
          }).
          catch(function() {
            console.log('FAILURE!!');
          });
      },
      onRegionSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetRegion?query=${search}`).then(response => {
          this.regions = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
      onCitySearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetCity?query=${search}&parentId=${this.currentRegion.id}`).then(response => {
          this.cities = response.data.result;
        }).finally(() => {
          loading(false,
          );
        });
      },
      onStreetSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetStreet?query=${search}&parentId=${this.currentCity.id}`).then(response => {
          this.streets = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
      onBuildingSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetBuilding?query=${search}&parentId=${this.currentStreet.id}`).then(response => {
          this.buildings = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
    },
    created() {
      if (this.$route.params.id !== undefined) {
        let url = `odata/RealEstateObject(${this.$route.params.id})`;
        axios.get(url).then(response => {
          this.item = response.data;
          return axios.get(`api/Address/GetParents?code=${this.item.Code}&building=${this.item.Building}`);
        }).then(response => {
          response.data.result[0].parents.forEach(parent => {
            if (parent.contentType === 'region') {
              this.currentRegion = parent;
            }
            if (parent.contentType === 'city') {
              this.currentCity = parent;
            }
            if (parent.contentType === 'street') {
              this.currentStreet = parent;
            }
          });
        }).catch(error => {

        }).finally(() => {

        });
      }
    },
  };
</script>

<style>
  .thumb {
    position: relative;
  }

  .thumb--image {
    max-height: 200px;
    object-fit: contain;

  }

  .file-input {
    display: none;
  }

  .v-select .dropdown-toggle {
    border: none;
  }

  .v-select ul.dropdown-menu {
    border: none;
  }
</style>
