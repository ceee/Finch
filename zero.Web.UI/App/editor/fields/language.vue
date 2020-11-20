<template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="onChange($event)" :disabled="disabled">
      <option :value="null"></option>
      <option v-for="item in items" :value="item.id">{{item.name}}</option>
    </select>
  </div>
</template>


<script>
  import LanguagesApi from 'zero/api/languages.js';

  export default {
    props: {
      value: {
        type: String
      },
      config: Object,
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      items: []
    }),

    mounted()
    {
      LanguagesApi.getForPicker().then(res =>
      {
        this.items = res;
      });
    },

    methods: {

      onChange(e)
      {
        this.$emit('input', e.target.value || null);
      }

    }
  }
</script>