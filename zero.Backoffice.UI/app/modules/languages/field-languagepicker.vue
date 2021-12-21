<template>
  <div class="ui-native-select" :disabled="config.disabled">
    <select :value="value" @input="onChange($event)" :disabled="config.disabled">
      <option :value="null"></option>
      <option v-for="item in items" :value="item.id">{{item.name}}</option>
    </select>
  </div>
</template>


<script>
  import api from './api';

  export default {
    props: {
      value: String,
      config: Object
    },

    data: () => ({
      items: []
    }),

    async mounted()
    {
      const result = await api.getByQuery({}, { system: this.config.system });
      this.items = result.data;
    },

    methods: {

      onChange(e)
      {
        this.$emit('input', e.target.value || null);
      }

    }
  }
</script>