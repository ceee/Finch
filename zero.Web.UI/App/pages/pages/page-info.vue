<template>
  <div class="page-editor-info ui-view-box has-sidebar">
    <div class="ui-box">
      box
    </div>
    <div class="ui-view-box-aside editor-infos">
      <div class="ui-box editor-active-toggle" :class="{'is-active': value.isActive }">
        <ui-property label="@ui.active" :is-text="true" class="is-toggle">
          <ui-toggle v-model="value.isActive" class="is-primary" />
        </ui-property>
      </div>
      <div class="ui-box is-light is-connected">
        <ui-property v-if="value.id" label="@ui.id" :is-text="true">
          {{value.id}}
        </ui-property>
        <ui-property v-if="value.id" label="@ui.createdDate" :is-text="true">
          <ui-date v-model="value.createdDate" />
        </ui-property>
        <ui-property label="Type" :is-text="true" v-if="pageType">
          <i :class="pageType.icon"></i> &nbsp;{{pageType.name}}
        </ui-property>
      </div>
    </div>
  </div>
</template>


<script>
  import PagesApi from 'zero/resources/pages';

  export default {
    props: {
      value: {
        type: [ Object, Array ]
      }
    },

    data: () => ({
      pageType: null
    }),

    mounted()
    {
      PagesApi.getPageType(this.value.pageTypeAlias).then(pageType =>
      {
        this.pageType = pageType;
      });
    }
  }
</script>