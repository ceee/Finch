<template>
  <div class="page-editor-info">
    <div class="ui-box" v-if="!isCreate && urls.length">
      <ui-property label="@page.infotab.links" :is-text="true" :vertical="false">
        <a v-for="url in urls" class="ui-link" :href="url" target="_blank"><ui-icon symbol="fth-external-link"></ui-icon> {{url}}</a>
      </ui-property>
      <!--<ui-property label="@page.infotab.revisions" :is-text="true" :vertical="false">-->
      <!--<ui-revisions :get="getRevisions" />-->
      <!--</ui-property>-->
    </div>
    <div class="ui-box">
      <!--<ui-property label="@page.schedule.label" :is-text="true" :vertical="false">
        <ui-daterangepicker :value="{ from: value.publishDate, to: value.unpublishDate }" @input="onRangeChange" :class="{ 'is-primary': value.publishDate || value.unpublishDate }" :disabled="disabled" />
      </ui-property>-->
      <ui-property label="@page.type" :is-text="true" v-if="pageType" :vertical="false">
        <ui-icon :symbol="pageType.icon" style="margin-right:6px;"></ui-icon>{{pageType.name}}
      </ui-property>
      <ui-property v-if="!isCreate" label="@ui.id" :is-text="true" :vertical="false">
        {{value.id}}
      </ui-property>
      <ui-property v-if="!isCreate" label="@ui.createdDate" :is-text="true" :vertical="false">
        <ui-date v-model="value.createdDate" />
      </ui-property>
      <ui-property v-if="!isCreate" label="@ui.modifiedDate" :is-text="true" :vertical="false">
        <ui-date v-model="value.lastModifiedDate" />
      </ui-property>
      <ui-property v-if="!isCreate" label="@ui.entityfields.alias" :is-text="true" :vertical="false">
        {{value.alias}}
      </ui-property>
    </div>
  </div>
</template>


<script>
  import api from '../api';

  export default {
    props: {
      value: {
        type: [ Object, Array ]
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      pageType: null,
      urls: []
    }),

    computed: {
      isCreate()
      {
        return !this.value.id;
      }
    },


    mounted()
    {
      //api.getPageType(this.value.pageTypeAlias).then(pageType =>
      //{
      //  this.pageType = pageType;
      //});
      //if (this.value.id)
      //{
      //  api.getUrls(this.value.id).then(urls =>
      //  {
      //    this.urls = urls;
      //  });
      //}
    },


    methods: {

      getRevisions(page)
      {
        //return api.getRevisions(this.value.id, page);
      },

      onRangeChange(value)
      {
        this.value.publishDate = value.from;
        this.value.unpublishDate = value.to;
      },
    }
  }
</script>

<style lang="scss">
  .page-editor-info
  {
    padding: 0 !important;
  }
  .page-editor-info .ui-view-box-aside
  {
    padding: 0;
  }
  .page-editor-info .ui-box
  {
    margin: 0;
  }
  .page-editor-info .ui-box + .ui-box
  {
    margin-top: var(--padding-s);
  }
  .page-editor-info .ui-property + .ui-property
  {
    /*border-top: none;
    margin-top: 0;
    padding-top: var(--padding-s);*/
  }
  .page-editor-info .ui-box:last-child
  {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }
</style>