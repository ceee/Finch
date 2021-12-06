<template>
  <ui-form ref="form" class="ui-daterangepicker" v-slot="form">
    <div class="ui-daterangepicker-items">
      <div :class="{ 'ui-split': config.options.rangeEnd }">
        <div class="ui-daterangepicker-group">
          <ui-property :label="config.options.fromText" :vertical="true">
            <ui-datepicker v-model="fromDate" :time="true" />
          </ui-property>
        </div>
        <div class="ui-daterangepicker-group" v-if="config.options.rangeEnd">
          <ui-property :label="config.options.toText" :vertical="true">
            <ui-datepicker v-model="toDate" :time="true" />
          </ui-property>
        </div>
      </div>
    </div>
    <div class="app-confirm-buttons">
      <ui-button type="primary" label="@ui.confirm" @click="confirm"></ui-button>
      <ui-button type="light" label="@ui.close" @click="config.close"></ui-button>
    </div>
  </ui-form>
</template>


<script>
  export default {

    props: {
      model: Object,
      config: Object,
    },

    data: () => ({
      disabled: false,
      fromDate: null,
      toDate: null
    }),


    mounted()
    {
      this.fromDate = this.model.from;
      this.toDate = this.model.to;
    },


    methods: {
      confirm()
      {
        this.config.confirm({
          from: this.fromDate,
          to: this.toDate
        }, this.config);
      }
    }
  }
</script>

<style lang="scss">
  .ui-daterangepicker
  {
    text-align: left;
  }

  h3.ui-daterangepicker-group-header
  {
    font-weight: 400;
    font-size: var(--font-size);
  }
</style>