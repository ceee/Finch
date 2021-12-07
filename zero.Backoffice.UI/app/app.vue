<template>
  <div class="app">
    <div>
      <input type="text" v-model="input" />
      <br />
      output: {{output}}
      <br />
      date: <span v-date="'2020-12-11T00:28:00'"></span>
      <br />
      name: <span v-localize="'@name'"></span>
      <br />
      version: {{zero.version}}
      <br />
      icon: <ui-icon symbol="fth-home" />
      <br />
      loading: <ui-loading />
      <br />
      tabs:
      <ui-tabs>
        <ui-tab label="Content">
          This is my content
        </ui-tab>
        <ui-tab :label="tabLabel">
          Our settings <button type="button" @click="tabLabel = 'New Settings'">change label</button>
        </ui-tab>
      </ui-tabs>
    </div>
  </div>
</template>


<script lang="ts">
  import './styles/styles';
  import { selectorToArray } from './utils';
  import { localize } from './services/localization';

  export default {
    data: () => ({
      input: null,
      name: localize('@name'),
      tabLabel: 'Settings'
    }),

    created()
    {
      console.info(this.zero.version);
    },

    computed: {
      output()
      {
        return selectorToArray(this.input);
      }
    }
  }

</script>