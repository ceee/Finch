<template>
  <div v-if="!loading" class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled">
      <option v-for="item in items" :value="item.code">{{item.name}}</option>
    </select>
  </div>
</template>


<script>
  import api from '../../ui/api';

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
      loading: true,
      items: []
    }),
    mounted()
    {
      api.getCultures().then(res =>
      {
        this.items = res.data;
        this.loading = false;
      });
    }
  }
</script>