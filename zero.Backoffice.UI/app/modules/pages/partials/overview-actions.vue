<template>
  <div class="page-overview">
    <button type="button" @click="action.action(action)" v-for="action in actions" :key="action.alias" class="page-overview-action">
      <div class="page-overview-action-icon">
        <ui-icon :symbol="action.icon" :size="24" />
      </div>
      <p class="page-overview-action-text">
        <strong v-localize="'@page.overview.actions.' + action.alias"></strong>
        <br>
        <span v-localize="{ key: '@page.overview.actions.' + action.alias + '_text', tokens: action.tokens }"></span>
      </p>
    </button>
  </div>
</template>

<script lang="ts">
  import api from '../api';
  import { defineComponent } from 'vue';
  import * as notifications from '../../../services/notification';

  export default defineComponent({

    name: 'pagesOverviewActions',

    emits: ['create'],

    data: () => ({
      actions: []
    }),


    created()
    {
      this.actions = [];

      var instance = this;

      let lastEditedPageId = localStorage.getItem('zero.last-page.hofbauer'); // TODO v3 + zero.appId);

      if (lastEditedPageId)
      {
        api.getById(lastEditedPageId).then(res =>
        {
          if (res.success && res.data)
          {
            this.actions.push({
              alias: 'continue',
              icon: 'fth-corner-down-right',
              tokens: {
                page: res.data.name
              },
              action()
              {
                instance.$router.push({
                  name: 'pages-edit',
                  params: { id: res.data.id }
                });
              }
            });
          }
        });
      }


      this.actions.push({
        alias: 'new',
        icon: 'fth-plus',
        tokens: {
          root: 'Home'
        },
        action()
        {
          instance.$emit('create')
        }
      });

      this.actions.push({
        alias: 'history',
        icon: 'fth-clock',
        action()
        {
          notifications.error('Not implemented', 'Page editing history has not been implemented yet');
        }
      });
    }

  });
</script>

<style lang="scss">

</style>