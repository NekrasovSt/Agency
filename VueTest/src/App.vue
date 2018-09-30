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
          <router-link class="mdl-navigation__link is-active" to="/">Главная</router-link>
          <a class="mdl-navigation__link" href="blog.html">Blog</a>
          <a class="mdl-navigation__link" href="about.html">About</a>
          <a class="mdl-navigation__link" href="contact.html">Contact</a>
        </nav>
      </div>
    </header>
    <div class="mdl-layout__drawer mdl-layout--small-screen-only">
      <nav class="mdl-navigation mdl-typography--body-1-force-preferred-font">
        <router-link class="mdl-navigation__link is-active" to="/">>Главная</router-link>
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
  export default {
    name: 'App'
  }
</script>

<style>

</style>
