<template>
  <div class="app-nav theme-light" :class="{'is-compact': compact }">

    <nav class="app-nav-inner">
      <template v-for="section in sections">
        <router-link :to="section.url" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <ui-icon :symbol="section.icon" class="app-nav-item-icon" :size="18" />
          <span class="app-nav-item-text" v-localize="section.name"></span>
          <ui-icon v-if="hasChildren(section)" symbol="fth-chevron-down" class="app-nav-item-arrow" />
        </router-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) === 0">
            <router-link v-for="child in section.children" v-bind:key="child.alias" :to="child.url" class="app-nav-child">
              <span class="app-nav-child-text" v-localize="child.name"></span>
            </router-link>
          </div>
        </transition>
      </template>
    </nav>

  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import AuthApi from 'zero/helpers/auth.js'
  import MediaApi from 'zero/api/media.js'
  import IconPicker from 'zero/components/pickers/iconPicker/iconpicker.vue';

  const compactCacheKey = 'zero.navigation.compact';

  export default {
    name: 'app-navigation',

    data: () => ({
      appId: zero.appId,
      applications: zero.applications,
      sections: zero.sections,
      user: null,
      userAvatar: null,
      compact: false,
      currentApplication: null
    }),


    components: { IconPicker },


    created()
    {
      this.currentApplication = _find(this.applications, x => x.id === zero.appId);
      this.compact = localStorage.getItem(compactCacheKey) === 'true';
    },


    methods: {

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      toggleSidebar()
      {
        this.compact = !this.compact;
        localStorage.setItem(compactCacheKey, this.compact.toString());
      }
    }
  }

</script>