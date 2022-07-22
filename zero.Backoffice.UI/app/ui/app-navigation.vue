<template>
  <div class="app-nav theme-dark" :class="{'is-compact': compact }">
    <div class="app-nav-boxed">
      <h1 class="app-nav-headline">
        <span class="app-nav-logo-circle"></span>
        <img src="/Assets/zero.svg" class="show-light" v-localize:alt="'@zero.name'" />
        <img src="/Assets/zero-dark.svg" class="show-dark" v-localize:alt="'@zero.name'" /> 
      </h1>
      
      <app-search />
    </div>

    <div v-if="currentApplication && appStore.applications.length > 0" class="app-nav-switch">
      <ui-button type="light block" :label="currentApplication.name" @click="$router.push('/')" />
    </div>
    <div v-else class="app-nav-switch is-fake"></div>

    <!--<ui-dropdown v-if="currentApplication && appStore.applications.length > 0" class="app-nav-switch">
      <template v-slot:button>
        <ui-button type="light block" :label="currentApplication.name" caret="right" />
      </template>
      <ui-dropdown-button v-for="app in appStore.applications" :value="app" :key="app.id" :label="app.name" :selected="app.id === appId" @click="applicationChanged" :prevent="true" />
      <ui-dropdown-separator />-->
      <!--<ui-dropdown-button :disabled="true" label="Add new application" icon="fth-plus" @click="addApplication" />-->
      <!--<ui-dropdown-button label="@nav.apps.manage" icon="fth-edit-2" @click="manageApplications" />
    </ui-dropdown>-->

    <nav class="app-nav-inner">
      <template v-for="section in ui.sections">
        <ui-link :to="section.url" :disabled="sectionDisabled(section)" class="app-nav-item" :alias="section.alias" :class="{ 'has-children': hasChildren(section) }">
          <ui-icon :symbol="section.icon" class="app-nav-item-icon" :size="18" />
          <span class="app-nav-item-text" v-localize="section.name"></span>
          <ui-icon v-if="hasChildren(section)" symbol="fth-chevron-down" class="app-nav-item-arrow" />
        </ui-link>
        <transition name="app-nav-children">
          <div class="app-nav-children" v-if="hasChildren(section) && $route.path.indexOf('/' + section.alias) === 0">
            <ui-link v-for="child in section.children" v-bind:key="child.alias" :alias="child.alias" :to="child.url" class="app-nav-child">
              <span class="app-nav-child-text" v-localize="child.name"></span>
              <span class="app-nav-child-count" v-if="child.count > 0">{{child.count}}</span>
            </ui-link>
          </div>
        </transition>
      </template>
    </nav>

    <ui-dropdown class="app-nav-account" v-if="account.user" align="left bottom">
      <template v-slot:button>
        <button type="button" class="app-nav-account-button">
          <ui-thumbnail class="-image" :media="account.user.avatarId" :alt="account.user.name">
            <ui-icon symbol="fth-user"></ui-icon>
          </ui-thumbnail>
          <p class="-text"><strong>{{account.user.name}}</strong></p>
          <ui-icon symbol="fth-more-horizontal" class="-arrow" />
        </button>
      </template>
      <!--<ui-dropdown-button label="@nav.account.edit" icon="fth-edit-2" @click="editUser" />
      <ui-dropdown-button label="@nav.account.changepassword" icon="fth-lock" @click="changePassword" />-->
      <!--<ui-dropdown-button label="Toggle sidebar" icon="fth-minimize-2" @click="toggleSidebar" />-->
      <ui-dropdown-button label="@nav.theme.dark" v-if="ui.preferences.theme !== 'dark'" icon="fth-moon" @click="ui.setTheme('dark')" />
      <ui-dropdown-button label="@nav.theme.light" v-else icon="fth-sun" @click="ui.setTheme('light')" />
      <ui-dropdown-button label="@nav.account.logout" icon="fth-log-out" @click="logout" />
    </ui-dropdown>

  </div>
</template>


<script>
  import { defineComponent } from 'vue';
  import { useAccountStore } from '../account/store';
  import { useUiStore } from '../ui/store';
  import { useAppStore } from '../modules/applications/store';
  import accountApi from '../account/api';
  import EventHub from '../services/eventhub';

  //const compactCacheKey = 'zero.navigation.compact';
  const themeCacheKey = 'zero.theme';

  export default defineComponent({
    name: 'app-navigation',

    data: () => ({
      account: null,
      ui: null,
      appStore: null,
      appId: null,
      applications: [],
      sections: [],
      compact: false,
      darkTheme: false,
      currentApplication: null,
      themeSwitchTimeout: null
    }),

    created()
    {
      this.ui = useUiStore();
      this.account = useAccountStore();
      this.appStore = useAppStore();
    },


    mounted()
    {
      this.currentApplication = this.appStore.applications[0];
      this.appId = this.currentApplication.id;
    },


    methods: {

      hasChildren(section)
      {
        return section.children && section.children.length > 0;
      },

      sectionDisabled(section)
      {
        return section.alias == 'dashboard';
        //return section.alias == 'pages' || section.alias == 'spaces';
      },

      editUser(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: 'users-edit',
          params: { id: this.user.id }
        });
      },

      changePassword(item, opts)
      {
        AuthApi.openPasswordOverlay();
        opts.hide();
      },

      async logout(item, opts)
      {
        await accountApi.logout();
        opts.hide();
        this.zero.events.emit('zero.authenticate');
      },

      addApplication(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: 'applications-create'
        });
      },

      manageApplications(item, opts)
      {
        opts.hide();
        this.$router.push({
          name: 'applications'
        });
      },

      applicationChanged(item, opts)
      {
        //opts.loading(true);

        //AuthApi.switchApp(item.id).then(success =>
        //{
        //  //opts.loading(false);
        //  //opts.hide();
        //});
      },

      toggleSidebar()
      {
        this.compact = !this.compact;
        localStorage.setItem(compactCacheKey, this.compact.toString());
      }
    }
  });

</script>


<style lang="scss">
  .app-nav-apps
  {
    position: absolute;
    left: 100%;
    top: 0;
    bottom: 0;
    width: 360px;
    background: var(--color-bg-shade-3);
    z-index: -1;
    margin-left: 0;
    display: flex;
    flex-direction: column;
    display: none; // TODO

    .ui-header-bar
    {
      height: 92px;
    }
  }

  .app-nav-app
  {
    display: grid;
    grid-template-columns: auto minmax(auto, 1fr) auto;
    grid-gap: var(--padding-s);
    align-items: center;
    margin: 0 var(--padding-m);
    padding: var(--padding-s) var(--padding-s);
    border-radius: var(--radius);

    &.is-active
    {
      background: var(--color-bg-shade-4);
      font-weight: 700;
    }
  }

  .app-nav-app-icon
  {
    height: 32px;
  }

  .app-nav-app-selected
  {
  }

  .app-nav
  {
    position: relative;
    background: var(--color-box);
    width: 260px;
    color: var(--color-text);
    height: 100vh;
    display: grid;
    grid-template-rows: auto auto 1fr auto;
    box-shadow: var(--shadow-short);
    margin-right: 1px;
    z-index: 5;

    .theme-rounded &
    {
      height: calc(100vh - 20px);
      margin: 10px;
      margin-right: 0;
      border-radius: var(--radius);
      box-shadow: var(--shadow-short);
    }

    &.theme-dark
    {
      //background-image: radial-gradient(rgba(255,255,255,.1) 1px,transparent 0),radial-gradient(rgba(255,255,255,.1) 1px,transparent 0);
      //background-size: 40px 40px;
      //background-position: 0 0,20px 20px;
    }
  }

  .app-nav-boxed
  {
    height: 90px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 var(--padding-xs) 0 0;
  }

  .app-nav-inner
  {
    overflow-y: auto;
    overflow-x: hidden;
  }

  .app-nav-headline
  {
    display: flex;
    align-items: center;
    padding: 0 var(--padding-m);
    margin: 0;

    .theme-rounded &
    {
      margin: 0;
    }

    img
    {
      //height: 24px;
      height: 15px;
      //margin-top: 9px;
    }
  }

  .app-nav-logo-circle
  {
    display: inline-block;
    width: 22px;
    height: 22px;
    border-radius: 20px;
    border: 4px solid var(--color-accent);
    margin-right: 12px;

    .theme-dark &
    {
      border-color: var(--color-text);
    }
  }

  .app-nav-search
  {
  }

  .app-nav-switch
  {
    margin-bottom: var(--padding-s);
    //border-bottom: 1px solid var(--color-line-onbg);
    background: var(--color-bg-shade-3);
    //background: var(--color-accent);

    .ui-button.type-light
    {
      padding: 0 24px;
      height: 70px;
      background: transparent;
      border-radius: 0;
      font-size: var(--font-size-m);
    }

    .ui-dropdown-button-icon
    {
      max-height: 20px;
      max-width: 20px;
    }

    &.is-fake
    {
      margin-bottom: 0;
      height: 0;
      background: none;
    }
  }

  a.app-nav-item, button.app-nav-item
  {
    display: grid;
    grid-template-columns: 28px 1fr auto;
    gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 var(--padding-m);
    height: 50px;
    color: var(--color-text);
    position: relative;

    & + .app-nav-item
    {
      margin-top: 1px;
    }

    &[aria-disabled]
    {
      opacity: .7;
    }

    &:hover
    {
      color: var(--color-text);
      background: var(--color-tree-selected);

      .app-nav-item-icon
      {
        color: var(--color-text);
      }
    }

    &.is-active:not([alias="dashboard"]), &.is-active-exact
    {
      color: var(--color-text);
      background: var(--color-tree-selected);
      font-weight: 700;
      //border-right: 3px solid var(--color-accent);

      .app-nav-item-icon
      {
        color: var(--color-text);
      }

      .app-nav-item-arrow
      {
        transform: rotate(180deg);
      }

      .app-nav-item-text
      {
        color: var(--color-text);
      }

      &:before
      {
        content: '';
        position: absolute;
        left: 0;
        top: 0;
        bottom: 0;
        //width: 3px;
        display: inline-block;
        background: var(--color-tree-selected-line);
      }
    }
  }

  .app-nav-item-text
  {
    transition: color 0.2s ease;
  }

  .app-nav-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -1px;
    color: var(--color-text);
    transition: color 0.2s ease;
  }

  .app-nav-item-arrow
  {
    color: var(--color-text-dim);
  }

  .app-nav-children
  {
    padding: 5px 0 10px;
  }

  a.app-nav-child
  {
    display: flex;
    align-items: center;
    font-size: var(--font-size);
    padding: 0 var(--padding) 0 calc(var(--padding) + 26px);
    height: 36px;
    color: var(--color-text-dim);
    position: relative;

    &:hover, &.is-active
    {
      color: var(--color-text);
    }

    &.is-active
    {
      font-weight: 700;
    }

    &.is-active:before
    {
      content: '';
      display: inline-block;
      width: 4px;
      height: 4px;
      border-radius: 4px;
      background: var(--color-accent);
      position: absolute;
      margin-left: -14px;
      margin-top: -3px;
      top: 50%;
    }
  }

  .app-nav-child-count
  {
    display: inline-block;
    font-size: 10px;
    font-weight: 700;
    text-transform: uppercase;
    background: var(--color-bg-shade-3);
    color: var(--color-text);
    height: 20px;
    line-height: 20px;
    padding: 0 8px;
    border-radius: 16px;
    letter-spacing: 0.5px;
    font-style: normal;
    margin-left: 10px;
    position: relative;
    top: 1px;
  }


  .app-nav-children-enter-active
  {
    transition: all .3s ease;
  }

  .app-nav-children-leave-active
  {
    transition: all 0;
  }

  .app-nav-children-enter, .app-nav-children-leave-to
  {
    transform: translateX(-10px);
    opacity: 0;
  }


  // account

  .app-nav-account
  {
    border-top: 1px solid var(--color-line-onbg);
  }

  .app-nav-account-button
  {
    display: grid;
    width: 100%;
    grid-template-columns: auto minmax(auto, 1fr) auto;
    gap: 16px;
    color: var(--color-text-dim);
    align-items: center;
    padding: var(--padding-m);
    border-bottom-left-radius: var(--radius);
    border-bottom-right-radius: var(--radius);

    &:hover
    {
      background: var(--color-bg-shade-2);
    }

    .-image
    {
      display: flex;
      align-items: center;
      justify-content: center;
      height: 32px;
      width: 32px;
      border-radius: 18px;
      position: relative;
      top: -1px;
      background: var(--color-bg-shade-3);
      text-align: center;
      font-size: 16px;
      overflow: hidden;
      color: var(--color-text);
    }

    .-text
    {
      font-weight: 400;
      margin: 0;

      strong
      {
        font-weight: 700;
        color: var(--color-text);
      }
    }
  }


  /* COMPACT MODE */

  .app-nav.is-compact
  {
    width: 82px;

    .app-nav-headline
    {
      width: 100%;
      overflow: hidden;
      padding-left: 0;
      padding-right: 0;

      img
      {
        margin-left: 29px;
        clip-path: circle(23.78% at 13px 14px);
        min-width: 118px;
      }
    }

    .app-nav-boxed
    {
      width: 100%;
      overflow: hidden;
    }

    .app-nav-switch
    {
      visibility: hidden;
      pointer-events: none;
      opacity: 0;
      width: 100%;
      overflow: hidden;
      padding: 16px 0 0;
    }

    a.app-nav-item, button.app-nav-item
    {
      display: flex;
      padding-left: var(--padding);
      width: 100%;
      //height: 60px;

      &:hover + .app-nav-children
      {
        display: block;
      }

      &:before
      {
        display: none;
      }
    }

    .app-nav-item-text, .app-nav-item-arrow
    {
      display: none;
    }

    .app-nav-children
    {
      display: none;
      position: absolute;
      z-index: 8;
      min-width: 240px;
      min-height: 20px;
      background: var(--color-dropdown);
      border-radius: var(--radius);
      border-top-left-radius: 0;
      border-bottom-left-radius: 0;
      border: 1px solid var(--color-dropdown-border);
      box-shadow: 6px 1px 8px rgba(0, 0, 0, 0.02);
      padding: 5px;
      color: var(--color-text);
      margin-left: 82px;
      margin-top: -55px;

      &:hover
      {
        display: block;
      }
    }

    a.app-nav-child
    {
      padding: 0 var(--padding);
      display: grid;
      width: 100%;
      grid-template-columns: minmax(0, 1fr) auto;
      gap: 6px;
      align-items: center;
      font-size: var(--font-size);
      padding: 0 16px;
      height: 48px;
      color: var(--color-text-dim);
      border-radius: var(--radius);
      white-space: nowrap;
      text-overflow: ellipsis;
      overflow: hidden;
      max-width: 100%;

      &:not([disabled]):hover, &:focus
      {
        background: var(--color-dropdown-selected);
      }

      &.is-active
      {
        color: var(--color-text);
        font-weight: 700;
      }
    }

    .app-nav-account
    {
      padding: 0;
      margin-bottom: var(--padding);
      margin-left: 25px;
    }

    .app-nav-account-button
    {
      display: block;
      width: 32px;

      .-text, .-arrow
      {
        display: none;
      }
    }
  }
</style>