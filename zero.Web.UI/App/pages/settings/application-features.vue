<template>
  <div class="application-features">
    <ui-property v-for="feature in features" :key="feature.alias" :label="feature.name" :description="feature.description">
      <ui-toggle :value="value.indexOf(feature.alias) > -1" @input="onFeatureToggle($event, feature)" :disabled="disabled" />
    </ui-property>
  </div>
</template>


<script>
  import ApplicationsApi from 'zero/resources/applications.js';

  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      },
      config: Object,
      disabled: {
        type: Boolean,
        default: false
      },
    },


    data: () => ({
      features: []
    }),


    created()
    {
      ApplicationsApi.getAllFeatures().then(items => this.features = items);
    },


    methods: {

      onFeatureToggle(isOn, feature)
      {
        const alias = feature.alias;
        const index = this.value.indexOf(alias);

        if (!isOn && index > -1)
        {
          this.value.splice(index, 1);
        }
        else if (isOn && index === -1)
        {
          this.value.push(alias);
        }

        this.$emit('input', this.value);
      }
    }
  }
</script>