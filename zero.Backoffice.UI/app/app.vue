<template>
  <div class="app">
    <div class="ui-box" style="width: 600px">
      nav:
      <router-link to="/">Home</router-link>
      <router-link to="/about">About</router-link>
      <router-link to="/countries">Countries</router-link><br />
      view:
      <router-view></router-view>
    </div>
    <div class="ui-box">
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
      <br /><br /><br />
      buttons:
      <ui-button label="Click" type="accent"></ui-button>
      <ui-icon-button icon="fth-home" type="accent"></ui-icon-button>
      <ui-dot-button></ui-dot-button>
      <ui-select-button label="Choose" description="Select an item"></ui-select-button>
      <ui-state-button :items="[{value: 'yes', label: 'Yes'}, {value: 'no', label: 'No'}]"></ui-state-button>
      <br />
      message: <ui-message text="Warning, this is not acceptable" />
      <br />
      pagination: <ui-pagination :pages="10" :page="2"></ui-pagination>
      <br />
      rte:
      <br />
      <ui-rte v-model="rte" />
      <br />
      <ui-toggle v-model="toggled" />
      <br />
      <ui-search v-if="toggled" v-model="search" @submit="searchSubmit($event)" />
      <br />
      countrypicker:
      <ui-countrypicker v-if="!toggled" />
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
      tabLabel: 'Settings',
      rte: '<p><b>Hallo</b><br>Das ist <i>schön</i></p><p>Ciao Tobi</p>',
      toggled: true,
      search: null
    }),

    computed: {
      output()
      {
        return selectorToArray(this.input);
      }
    },

    methods: {
      searchSubmit(ev)
      {
        console.info('searched: ' + ev);
      }
    }
  }

</script>