<template>
  <div class="ui-revisions" :class="{ 'is-empty': value.items.length < 1 }">
    <div class="ui-revision" v-for="revision in value.items">
      <span class="ui-revision-action">updated</span>
      <ui-date class="ui-revision-date" v-model="revision.date" format="long" :split="true" />
      <router-link :to="{ name: userRoute, params: { id: revision.user.id }}" v-if="revision.user" class="ui-revision-user">
        <img class="ui-revision-user-image" v-if="revision.user" :src="getImage(revision.user.avatarId)" :alt="revision.user.name" />
        <span v-if="revision.user" class="ui-revision-user-name">{{revision.user.name}}</span>
      </router-link>
      <div v-else></div>
      <button class="ui-link">View</button>
    </div>
  </div>
</template>


<script>
  import dayjs from 'dayjs';
  import Strings from 'zero/services/strings';
  import MediaApi from 'zero/resources/media.js';

  export default {
    name: 'uiRevisions',

    props: {
      value: {
        type: Object,
        default: () => ({
          totalItems: 0,
          items: []
        })
      }
    },


    data: () => ({
      userRoute: zero.alias.sections.settings + '-' + zero.alias.settings.users + '-edit'
    }),


    methods: {
      getImage(id)
      {
        return MediaApi.getImageSource(id);
      }
    }

  }
</script>


<style lang="scss">
  .ui-revision
  {
    display: grid;
    grid-template-columns: auto 3fr 2fr auto;
    grid-gap: 20px;
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
    background: var(--color-accent-info-bg);
    color: var(--color-accent-info);
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
      background: var(--color-accent-info-bg);
    }
  }

  .ui-revision-user
  {
    color: var(--color-fg);
  }

  .ui-revision-user-image
  {
    width: 28px;
    height: 28px;
    border-radius: 14px;
    margin-right: 8px;
  }
</style>