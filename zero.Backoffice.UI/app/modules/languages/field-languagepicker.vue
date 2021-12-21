<template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="onChange($event)" :disabled="disabled">
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
      entity: Object,
      disabled: Boolean
    },

    data: () => ({
      items: []
    }),

    async mounted()
    {
      const result = await api.getByQuery({});
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