<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/apartment.jpeg" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Квартира</h2>
      </div>
      <div class="mdl-card__supporting-text">
        Продажа вторичного жилья
      </div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     to="/object-list/apartment">Read more
        </router-link>
      </div>
    </div>
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/building.jpeg" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Новостройка</h2>
      </div>
      <div class="mdl-card__supporting-text">
        Все новостройки
      </div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     to="/object-list/newBuilding">Read more
        </router-link>
      </div>
    </div>
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/house.jpeg" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Дом</h2>
      </div>
      <div class="mdl-card__supporting-text">
        Покупка/продажа домов
      </div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     to="/object-list/house">Read more
        </router-link>
      </div>
    </div>
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/mortgage.jpeg" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Ипотека</h2>
      </div>
      <div class="mdl-card__supporting-text">Военная ипотек</div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link to="/mortgage"
                     class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     href="portfolio-example01.html">Read more
        </router-link>
      </div>
    </div>
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/certificate.png" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Сертификаты</h2>
      </div>
      <div class="mdl-card__supporting-text">
        Работа с сертификатами любой сложности
      </div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     to="/certificate">Read more
        </router-link>
      </div>
    </div>
    <div class="mdl-cell mdl-card mdl-shadow--4dp portfolio-card">
      <div class="mdl-card__media">
        <img class="article-image" src="@/assets/support.jpeg" border="0" alt="">
      </div>
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Сопровождение</h2>
      </div>
      <div class="mdl-card__supporting-text">
        Юридическое сопровождение любой сложности
      </div>
      <div class="mdl-card__actions mdl-card--border">
        <router-link class="mdl-button mdl-button--colored mdl-js-button mdl-js-ripple-effect mdl-button--accent"
                     to="/support">Read more
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
  //https://www.pexels.com/photo/round-brown-wooden-stool-near-brown-sofa-892618/       Photo by Daria Shevtsova from Pexels

  //https://www.pexels.com/photo/apartment-apartment-building-architecture-building-323705/ Photo by Expect Best from Pexels
  //https://www.pexels.com/photo/person-giving-keys-on-man-1288482/ Photo by rawpixel.com from Pexels
  //https://www.pexels.com/photo/architect-composition-data-demonstration-313691/ Photo by energepic.com from Pexels

  export default {
    name: "MainPage"
  }
</script>

<style scoped>
  .portfolio-card > .mdl-card__actions {
    margin-top: auto;
  }
</style>
