<template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled">
      <option v-for="item in items" :value="item.code">{{item.name}}</option>
    </select>
  </div>
</template>


<script>
  import LanguagesApi from 'zero/resources/languages.js';

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
      LanguagesApi.getSupportedCultures().then(res =>
      {
        this.items = res
      });
    }
  }
</script>