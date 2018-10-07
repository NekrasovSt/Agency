<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h1 class="mdl-card__title-text">Заполните форму</h1>
      </div>
      <div class="mdl-card__supporting-text  mdl-grid">
        <div class="mdl-cell mdl-cell--4-col mdl-card__media thumb" v-for="(file, index) in images">
          <img :src="file.blob" class="article-image thumb--image"/>
          <div class="mdl-card__menu">
            <button class="mdl-button mdl-button--icon mdl-js-button mdl-js-ripple-effect"
                    @click.prevent="remove(file)">
              <i class="material-icons">delete</i>
            </button>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--12-col">
          <file-upload
            class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
            ref="upload"
            v-model="images"
            :multiple="true"
            @input-filter="inputFilter"
            @input-file="inputFile"
            post-action="/api/upload">
            Загрузит файл
          </file-upload>
          <button v-show="!$refs.upload || !$refs.upload.active" @click.prevent="$refs.upload.active = true"
                  type="button">Start upload
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
        images: []
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
        this.$refs.upload.remove(file)
      },
      inputFile(newFile, oldFile) {
        if (newFile && !oldFile) {
          // Add file
        }

        if (newFile && oldFile) {
          // Update file

          // Start upload
          if (newFile.active !== oldFile.active) {
            console.log('Start upload', newFile.active, newFile);

          }

          // Upload progress
          if (newFile.progress !== oldFile.progress) {
            console.log('progress', newFile.progress, newFile)
          }

          // Upload error
          if (newFile.error !== oldFile.error) {
            console.log('error', newFile.error, newFile)
          }

          // Uploaded successfully
          if (newFile.success !== oldFile.success) {
            console.log('success', newFile.success, newFile)
          }
        }

        if (!newFile && oldFile) {
          // Remove file

          // Automatically delete files on the server
          if (oldFile.success && oldFile.response) {
            // $.ajax({
            //   type: 'DELETE',
            //   url: '/file/delete?id=' + oldFile.response.id,
            // });
            console.log('delete', oldFile.response);
          }
        }
      },
      inputFilter: function (newFile, oldFile, prevent) {
        if (newFile && !oldFile) {
          // Filter non-image file
          if (!/\.(jpeg|jpe|jpg|gif|png|webp)$/i.test(newFile.name)) {
            return prevent()
          }
        }

        if (newFile && (!oldFile || newFile.file !== oldFile.file)) {
          newFile.blob = '';
          let URL = window.URL || window.webkitURL;
          if (URL && URL.createObjectURL) {
            newFile.blob = URL.createObjectURL(newFile.file)
          }
        }
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

  .v-select .dropdown-toggle {
    border: none;
  }

  .v-select ul.dropdown-menu {
    border: none;
  }
</style>
