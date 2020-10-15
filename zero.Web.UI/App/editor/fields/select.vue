 <template>
  <div class="ui-native-select" :disabled="disabled">
    <select :value="value" @input="$emit('input', $event.target.value)" :disabled="disabled">
      <option v-for="item in config.items" :value="item.value" v-localize="item.label"></option>
    </select>
  </div>
</template>


<script>
  import { map as _map, filter as _filter } from 'underscore';

  export default {
    name: 'uiSelect',

    props: {
      value: {
        type: [String, Number, Object],
        default: null
      },
      config: Object,
      disabled: {
        type: Boolean,
        default: false
      }
    },
  }
</script>