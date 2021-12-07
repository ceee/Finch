<template>
  <div class="ui-revisions" :class="{ 'is-empty': items.length < 1 }">
    <div class="ui-revision" v-for="revision in items">
      <span class="ui-revision-action" v-localize="'@revisions.actions.updated'"></span>
      <ui-date class="ui-revision-date" v-model="revision.date" format="long" :split="true" />
      <router-link :to="{ name: userRoute, params: { id: revision.user.id }}" v-if="revision.user" class="ui-revision-user">
        <img class="ui-revision-user-image" v-if="revision.user" :src="getImage(revision.user.avatarId)" :alt="revision.user.name" />
        <span v-if="revision.user" class="ui-revision-user-name">{{revision.user.name}}</span>
      </router-link>
      <div v-else></div>
      <button type="button" class="ui-link is-minor" v-localize="'@revisions.view'"></button>
    </div>
    <ui-pagination v-if="pages > 1" :pages="pages" :page="page" @change="setPage" />
  </div>
</template>


<script>
  import dayjs from 'dayjs';
  import Strings from 'zero/helpers/strings.js';
  import MediaApi from 'zero/api/media.js';

  export default {
    name: 'uiRevisions',

    props: {
      get: {
        type: Function,
        required: true
      }
    },


    data: () => ({
      userRoute: zero.alias.settings.users + '-edit',
      items: [],
      page: 1,
      pages: 0
    }),


    mounted()
    {
      this.setPage(this.page);
    },


    methods: {
      getImage(id)
      {
        return MediaApi.getImageSource(id);
      },

      setPage(page)
      {
        this.page = page;
        this.get(page).then(res =>
        {
          this.items = res.items;
          this.pages = res.totalPages;
        });
      }
    }
  }
</script>


<style lang="scss">
  .ui-revision
  {
    display: grid;
    grid-template-columns: auto 3fr 2fr auto;
    gap: 20px;
    align-items: center;
    min-height: 28px;

    & + .ui-revision
    {
      margin-top: 24px;
    }

    &:first-child .ui-revision-action:before
    {
      display: none;
    }
  }

  .ui-revision-action
  {
    align-self: center;
    display: inline-block;
    font-size: 9px;
    font-weight: 700;
    text-transform: uppercase;
    background: var(--color-box-nested);
    color: var(--color-text-dim);
    height: 22px;
    line-height: 22px;
    padding: 0 10px;
    border-radius: 16px;
    letter-spacing: .5px;
    position: relative;

    &:before
    {
      content: '';
      position: absolute;
      left: calc(50% - 1.5px);
      top: -30px;
      height: 30px;
      width: 3px;
      background: var(--color-box-nested);
    }
  }

  .ui-revision-user
  {
    color: var(--color-text);
  }

  .ui-revision-user-image
  {
    width: 28px;
    height: 28px;
    border-radius: 14px;
    margin-right: 8px;
  }
</style>