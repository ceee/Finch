<template>
  <div class="app-nav">

    <h1 class="app-nav-headline" v-localize="'@zero.name'">zero</h1>

    <div class="app-nav-switch">
      <ui-button v-if="applications.length > 0" type="action block" :label="applications[0].name" caret="down" />
    </div>

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="getLink(section)" class="app-nav-item" :class="{ 'has-children': hasChildren(section) }">
          <i class="app-nav-item-icon" :class="section.icon" :style="{ color: false && section.color ? section.color : null }"></i>
          {{section.name | localize}}
          <i v-if="hasChildren(section)" class="app-nav-item-arrow fth-chevron-down"></i>
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) > -1">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="getLink(section, child)" class="app-nav-child">
              {{child.name | localize}}
            </router-link>
          </div>
        </transition>
      </template>
    </nav>

    <footer class="app-nav-account">
      <button type="button" class="app-nav-account-button">
        <img class="-image" src="https://fifty.brothers.studio/Media/Avatars/tobi.jpg" alt="Tobi" />
        <p class="-text"><strong>Tobias Klika</strong><br>Admin</p>
        <i class="-arrow fth-chevron-down"></i>
      </button>
    </footer>

  </div>
</template>


<script>
  import ApplicationsApi from 'zeroresources/applications.js'
  import SectionsApi from 'zeroresources/sections.js'

  export default {
    name: 'app-navigation',

    data: () => ({
      applications: [],
      sections: []
    }),

    mounted ()
    {
      //console.info(this.$router.history.current.path);

      SectionsApi.getAll().then(items =>
      {
        this.sections = items;
      });
      ApplicationsApi.getAll().then(items =>
      {
        this.applications = items;
      });
    },

    methods: {

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      getName(section)
      {
        return section.alias.charAt(0).toUpperCase() + section.alias.slice(1);
      },

      getLink(section, child)
      {
        //if (section.alias === "dashboard" && !child)
        //{
        //  return '/';
        //}

        if (!child)
        {
          return '/' + section.alias;
        }

        return '/' + section.alias + '/' + child.alias;
      }

    }
  }

</script>